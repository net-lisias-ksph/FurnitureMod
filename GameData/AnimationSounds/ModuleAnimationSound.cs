using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimationSounds
{
    public class ModuleAnimationSounds : PartModule
    {
        [KSPField]
        public string fileMoving;
        [KSPField]
        public string fileStop;
        [KSPField]
        public bool loopMoving = false;

        private AudioSource soundMoving;
        private AudioSource soundStop;
        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
            enabled = false;

            ModuleAnimateGeneric module = part.FindModuleImplementing<ModuleAnimateGeneric>();
            if (module != null)
            {
                soundMoving = gameObject.AddComponent<AudioSource>();
                soundMoving.clip = GameDatabase.Instance.GetAudioClip(fileMoving);
                soundStop = gameObject.AddComponent<AudioSource>();
                soundStop.clip = GameDatabase.Instance.GetAudioClip(fileStop);

                soundMoving.loop = loopMoving;
                soundMoving.volume = GameSettings.AMBIENCE_VOLUME;
                soundMoving.dopplerLevel = 0f;
                soundMoving.rolloffMode = AudioRolloffMode.Logarithmic;
                soundMoving.minDistance = 0.5f;
                soundMoving.maxDistance = 1f;

                soundStop.loop = false;
                soundStop.volume = GameSettings.AMBIENCE_VOLUME;
                soundStop.dopplerLevel = 0f;
                soundStop.rolloffMode = AudioRolloffMode.Logarithmic;
                soundStop.minDistance = 0.5f;
                soundStop.maxDistance = 1f;

                module.OnMoving.Add(new EventData<float, float>.OnEvent(OnMoving));
                module.OnStop.Add(new EventData<float>.OnEvent(OnStop));

                GameEvents.onGamePause.Add(Pause);
                GameEvents.onGameUnpause.Add(Unpause);
            }

        }
        public void Destroy()
        {
            GameEvents.onGamePause.Remove(Pause);
            GameEvents.onGameUnpause.Remove(Unpause);
        }
        private void Pause()
        {
            if (soundMoving != null && soundMoving.isPlaying)
                soundMoving.Stop();
            if (soundStop != null && soundStop.isPlaying)
                soundStop.Stop();
        }
        private void Unpause()
        {

        }
        private void OnMoving(float a, float b)
        {
            soundMoving.Play();
        }
        private void OnStop(float a)
        {
            soundMoving.Stop();
            soundStop.Play();
        }

    }
}
