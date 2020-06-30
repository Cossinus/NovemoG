using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Novemo.Characters.Player;
using Novemo.Controllers;
using Novemo.Inventories;
using TMPro;
using UnityEngine;

namespace Novemo.UI
{
	public class EventLog : MonoBehaviour
	{
		#region Singleton

		public static EventLog Instance;
		
		private void Awake()
		{
			Instance = this;
		}

		#endregion
		
		public int visibleTime;
		
		public int eventsMaxCount;

		public Transform parent;
		
		public GameObject eventLogPrefab;

		private List<GameObject> eventLogs = new List<GameObject>();

		private static Inventory playerInventory;

		public event Action<string> OnEventLog;

		public void RaiseEventLog(string log) => OnEventLog?.Invoke(log);

		private void Start()
		{
			OnEventLog += OnEventLogged;
			playerInventory = PlayerManager.Instance.player.GetComponent<PlayerController>().inventory;
		}

		private void OnEventLogged(string log)
		{
			if (eventLogs.Count == eventsMaxCount)
			{
				StartCoroutine(DisableLog(eventLogs.Last(), 0));
			}
			
			parent.transform.SetParent(playerInventory.IsOpen ? playerInventory.transform : transform);

			var addedLog = Instantiate(eventLogPrefab, parent);
			addedLog.GetComponent<TextMeshProUGUI>().text = log;
			addedLog.transform.Find("LogTextVisual").GetComponent<TextMeshProUGUI>().text = log;
			addedLog.transform.SetAsFirstSibling();

			eventLogs.Insert(0, addedLog);
			
			StartCoroutine(DisableLog(addedLog, visibleTime));

			//Pull in animation
		}

		private IEnumerator DisableLog(GameObject eventLog, float visTime)
		{
			yield return new WaitForSeconds(visTime);
			
			try
			{
				//Fade off animation
				Destroy(eventLog);
				eventLogs.Remove(eventLog);
			} catch { /*Ignored*/ }
		}
	}
}