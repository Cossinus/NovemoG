using System;
using UnityEngine;

namespace Novemo.UI
{
	public class EventLog : MonoBehaviour
	{
		public int eventsMaxCount;

		public GameObject eventLogPrefab;
		
		public event Action<string> OnEventLog;

		private void Start()
		{
			OnEventLog += OnEventLogged;
		}

		private void OnEventLogged(string log)
		{
			var eventLogText = Instantiate(eventLogPrefab, transform.parent)
		}
	}
}