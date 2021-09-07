using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
	public enum Sound
	{
		PlayerAttack,
		PlayerHit,
		EnemyHit,
		PlayerDie,
		EnemyDie,
		PickupItem,
		PickupCurrency,
		SpellCast,
		EnemyProjectile,
		LevelUp,
		Achievement,
	}

	private static GameObject oneShotGameObject;
	private static AudioSource oneShotAudioSource;

	public static void PlaySound(Sound sound)
	{
		if(oneShotGameObject == null)
		{
			oneShotGameObject = new GameObject("One Shot Sound");
			oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
		}
		oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
	}

	private static AudioClip GetAudioClip(Sound sound)
	{
		foreach (GameManager.SoundAudioClip soundAudioClip in GameManager.GameManagerInstance.soundAudioClipArray)
		{
			if (soundAudioClip.sound == sound)
			{
				return soundAudioClip.audioClip;
			}
		}
		return null;
	}
}