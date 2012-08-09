using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Omega
{
    public class AudioManager : Actor
    {      
        Dictionary<string, SoundEffect> sounds;
        Dictionary<string, Song> songs;   

        public bool SoundEnabled;

        public bool MusicEnabled
        {
            get { return !MediaPlayer.IsMuted; }
            set { if(MediaPlayer.GameHasControl) MediaPlayer.IsMuted = !value; }
        }

        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { if(MediaPlayer.GameHasControl) MediaPlayer.Volume = value; }
        }

        public MediaState State
        {
            get { return MediaPlayer.State; }
        }

        AudioListener listener;

        public Actor TargetListener;

        public AudioManager(bool soundEnabled, float soundVolume, bool musicEnabled, float musicVolume) : base()
        {
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
            
            listener = new AudioListener();
            listener.Forward = Vector3.Forward;
            listener.Up = Vector3.Up;

            SoundEnabled = soundEnabled;
            SoundVolume = soundVolume;
            MusicEnabled = musicEnabled;
            MusicVolume = musicVolume;

            SoundEffect.DistanceScale = 300.0f;
        }
        
        public void LoadSound(string path, ContentManager contentManager)
        {
            sounds.Add(path, contentManager.Load<SoundEffect>(path));
        }

        public void LoadSong(string path, ContentManager contentManager)
        {
            songs.Add(path, contentManager.Load<Song>(path));
        }

        public void PlaySound(string name)
        {
            if(SoundEnabled)
                sounds[name].Play();
        }

        public void PlaySound(string name, float volume, float pitch, float pan)
        {
            if(SoundEnabled)
                sounds[name].Play(volume, pitch, pan);
        }

        public void PlaySound(string name, Vector3 emitterPosition, Vector3 emitterVelocity)
        {
            if (SoundEnabled)
            {
                AudioEmitter emitter = new AudioEmitter();
                emitter.Position = emitterPosition;
                emitter.Velocity = emitterVelocity;
                emitter.Forward = Vector3.Forward;
                emitter.Up = Vector3.Up;

                SoundEffectInstance instance = sounds[name].CreateInstance();
                instance.Apply3D(listener, emitter);
                instance.Play();
            }
        }

        public void PlaySong(string name, bool isRepeating)
        {
            if (MusicEnabled && MediaPlayer.GameHasControl && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(songs[name]);
                MediaPlayer.IsRepeating = isRepeating;
            }
        }

        public void Pause()
        {
            if (MusicEnabled && MediaPlayer.GameHasControl && MediaPlayer.State != MediaState.Paused)
                MediaPlayer.Pause();
        }

        public void Resume()
        {
            if (MusicEnabled && MediaPlayer.GameHasControl && MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Resume();
        }

        public void Stop()
        {
            if (MusicEnabled && MediaPlayer.GameHasControl && MediaPlayer.State != MediaState.Stopped)
                MediaPlayer.Stop();
        }

        public override void Update(GameTimerEventArgs e)
        {
            if (TargetListener != null)
            {
                listener.Position = TargetListener.Position;
                listener.Velocity = TargetListener.Velocity;
            }

            base.Update(e);
        }
    }
}
