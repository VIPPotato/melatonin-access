using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steamworks;
using UnityEngine;

public class SteamWorkshop : Custom
{
	public struct subscribedItem
	{
		public string title;

		public string author;

		public string tags;

		public string path;

		public CSteamID authorId;

		public PublishedFileId_t workshopItemId;
	}

	public static SteamWorkshop mgr;

	private string uploadTitle;

	private string uploadDescription;

	private List<string> uploadTags;

	private const string tempFolderName = "TempWorkshop";

	private string tempFolderPath;

	private string thumbnailPath;

	private string itemsFolderPath;

	private bool isUploading;

	private bool isDownloading;

	private bool isLastUploadSuccessful;

	public Dictionary<PublishedFileId_t, subscribedItem> subscribedItemsDictionary = new Dictionary<PublishedFileId_t, subscribedItem>();

	private static AppId_t myAppId => (AppId_t)1585220u;

	public void Awake()
	{
		mgr = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		tempFolderPath = Application.dataPath + "/TempWorkshop";
		thumbnailPath = Application.dataPath + "/thumbnail.png";
		tempFolderPath = tempFolderPath.Replace("/Melatonin_Data", "");
		thumbnailPath = thumbnailPath.Replace("/Melatonin_Data", "");
		itemsFolderPath = Application.dataPath.Replace("common/Melatonin/Melatonin_Data", "workshop/content/1585220");
	}

	public void Upload(string newUploadTitle)
	{
		if (!(SteamManager.mgr != null) || !SteamManager.Initialized)
		{
			return;
		}
		if (isUploading)
		{
			Debug.Log("Workshop Item Submission Already In Progress");
			return;
		}
		isLastUploadSuccessful = false;
		isUploading = true;
		uploadTitle = newUploadTitle;
		uploadDescription = "An edited version of " + GetLevelNameFromScene() + ".";
		uploadTags = new List<string> { GetLevelNameFromScene() };
		string text = Application.dataPath + "/" + SceneMonitor.mgr.GetActiveSceneName();
		text = text.Replace("/Melatonin_Data", "");
		string[] array;
		if (File.Exists(text + ".mp3") || File.Exists(text + ".wav") || File.Exists(text + ".ogg"))
		{
			string text2 = "";
			if (File.Exists(text + ".mp3"))
			{
				text2 = text + ".mp3";
			}
			else if (File.Exists(text + ".wav"))
			{
				text2 = text + ".wav";
			}
			else if (File.Exists(text + ".ogg"))
			{
				text2 = text + ".ogg";
			}
			array = new string[2]
			{
				text + ".json",
				text2
			};
			uploadTags.Add("Custom Music");
		}
		else
		{
			array = new string[1] { text + ".json" };
			uploadTags.Add("Melatonin OST");
		}
		if (Directory.Exists(tempFolderPath))
		{
			Directory.Delete(tempFolderPath, recursive: true);
		}
		Directory.CreateDirectory(tempFolderPath);
		for (int i = 0; i < array.Length; i++)
		{
			if (File.Exists(array[i]))
			{
				string text3 = array[i];
				string text4 = array[i].Replace(SceneMonitor.mgr.GetActiveSceneName(), "TempWorkshop/" + SceneMonitor.mgr.GetActiveSceneName());
				Debug.Log("Moving: \"" + text3 + "\" to \"" + text4 + "\"");
				File.Copy(text3, text4, overwrite: true);
				continue;
			}
			isLastUploadSuccessful = false;
			isUploading = false;
			return;
		}
		StartCoroutine(WaitABit(delegate
		{
			UploadWorkshopItem(EWorkshopFileType.k_EWorkshopFileTypeFirst, OnComplete);
		}));
		void CreateItem(EWorkshopFileType workshopFiletype, Action<PublishedFileId_t> callback)
		{
			CallResult<CreateItemResult_t> callResult = CallResult<CreateItemResult_t>.Create(delegate(CreateItemResult_t createItemResult_t, bool failed)
			{
				if (createItemResult_t.m_bUserNeedsToAcceptWorkshopLegalAgreement)
				{
					SteamFriends.ActivateGameOverlayToWebPage("https://steamcommunity.com/sharedfiles/workshoplegalagreement");
				}
				Debug.Log(createItemResult_t.m_eResult);
				Debug.Log($"PublishedFileId_t: {createItemResult_t.m_nPublishedFileId}");
				if (createItemResult_t.m_eResult == EResult.k_EResultOK)
				{
					callback(createItemResult_t.m_nPublishedFileId);
				}
				else
				{
					if (Directory.Exists(tempFolderPath))
					{
						Directory.Delete(tempFolderPath, recursive: true);
					}
					isUploading = false;
				}
			});
			SteamAPICall_t hAPICall = SteamUGC.CreateItem(myAppId, workshopFiletype);
			callResult.Set(hAPICall);
		}
		void OnComplete(PublishedFileId_t itemId)
		{
			Directory.Delete(tempFolderPath, recursive: true);
			SteamFriends.ActivateGameOverlayToWebPage($"steam://url/CommunityFilePage/{itemId}");
		}
		void SubmitItemUpdate(UGCUpdateHandle_t updateHandle, Action<SubmitItemUpdateResult_t> callback)
		{
			CallResult<SubmitItemUpdateResult_t> callResult = CallResult<SubmitItemUpdateResult_t>.Create(delegate(SubmitItemUpdateResult_t obj, bool failed)
			{
				if (!failed)
				{
					callback(obj);
				}
			});
			SteamUGC.SetItemTitle(updateHandle, uploadTitle);
			SteamUGC.SetItemDescription(updateHandle, uploadDescription);
			SteamUGC.SetItemUpdateLanguage(updateHandle, "english");
			SteamUGC.SetItemMetadata(updateHandle, "");
			SteamUGC.SetItemVisibility(updateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
			SteamUGC.SetItemTags(updateHandle, uploadTags);
			SteamUGC.SetItemContent(updateHandle, tempFolderPath);
			if (File.Exists(thumbnailPath))
			{
				if (new FileInfo(thumbnailPath).Length < 102400)
				{
					SteamUGC.SetItemPreview(updateHandle, thumbnailPath);
				}
				else
				{
					SteamUGC.SetItemPreview(updateHandle, "");
				}
			}
			Debug.Log($"SubmitItemUpdate: {updateHandle}");
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(updateHandle, null);
			callResult.Set(hAPICall);
		}
		IEnumerator UpdateRoutine(UGCUpdateHandle_t updateHandle)
		{
			EItemUpdateStatus previousStatus = EItemUpdateStatus.k_EItemUpdateStatusInvalid;
			ulong previousProgress = 0uL;
			int staleUploadCount = 0;
			while (!isLastUploadSuccessful && isUploading)
			{
				yield return new WaitForSeconds(1f);
				if (!isUploading)
				{
					break;
				}
				ulong punBytesProcessed;
				ulong punBytesTotal;
				EItemUpdateStatus itemUpdateProgress = SteamUGC.GetItemUpdateProgress(updateHandle, out punBytesProcessed, out punBytesTotal);
				staleUploadCount = ((previousProgress == punBytesProcessed && previousStatus == itemUpdateProgress) ? (staleUploadCount + 1) : 0);
				if (staleUploadCount >= 10)
				{
					Debug.Log($"UploadWorkshopItem Failed. Last Status {itemUpdateProgress} - Progress: [{punBytesProcessed}/{punBytesTotal}]");
					isUploading = false;
					break;
				}
				previousProgress = punBytesProcessed;
				previousStatus = itemUpdateProgress;
				Debug.Log($"submit status: {itemUpdateProgress}. [{punBytesProcessed}/{punBytesTotal}]");
			}
			yield return null;
		}
		void UploadWorkshopItem(EWorkshopFileType workshopFileType, Action<PublishedFileId_t> callback)
		{
			Debug.Log("CreateItem");
			CreateItem(workshopFileType, delegate(PublishedFileId_t fileId)
			{
				Debug.Log($"StartItemUpdate: {fileId}");
				UGCUpdateHandle_t uGCUpdateHandle_t = SteamUGC.StartItemUpdate((AppId_t)1585220u, fileId);
				Debug.Log($"UGCUpdateHandle_t: {uGCUpdateHandle_t}");
				Debug.Log("SubmitItemUpdate");
				SubmitItemUpdate(uGCUpdateHandle_t, delegate(SubmitItemUpdateResult_t updateResult)
				{
					Debug.Log($"Complete {updateResult.m_eResult}");
					isLastUploadSuccessful = true;
					callback(updateResult.m_nPublishedFileId);
					isUploading = false;
				});
				StartCoroutine(UpdateRoutine(uGCUpdateHandle_t));
			});
		}
		static IEnumerator WaitABit(Action callback)
		{
			yield return new WaitForSeconds(0.5f);
			callback();
			yield return null;
		}
	}

	public void OpenWorkshopFrontPage()
	{
		SteamFriends.ActivateGameOverlayToWebPage(string.Format("{0}//steamcommunity.com/app/{1}/workshop/", "https:", myAppId));
	}

	public void OpenDiscord()
	{
		SteamFriends.ActivateGameOverlayToWebPage("https://discord.gg/3s4mRqyyQz");
	}

	public void OpenBluesky()
	{
		SteamFriends.ActivateGameOverlayToWebPage("https://bsky.app/profile/halfasleep.games");
	}

	public void DownloadSubscribedItems()
	{
		isDownloading = true;
		subscribedItemsDictionary = new Dictionary<PublishedFileId_t, subscribedItem>();
		uint itemCount = SteamUGC.GetNumSubscribedItems();
		PublishedFileId_t[] pvecPublishedFileID = new PublishedFileId_t[itemCount];
		SteamUGC.GetSubscribedItems(pvecPublishedFileID, itemCount);
		SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(SteamUGC.CreateQueryUGCDetailsRequest(pvecPublishedFileID, itemCount));
		uint validItemCount = 0u;
		CallResult<SteamUGCQueryCompleted_t>.Create(delegate(SteamUGCQueryCompleted_t result, bool ioFailed)
		{
			for (uint num = 0u; num < itemCount; num++)
			{
				if (SteamUGC.GetQueryUGCResult(result.m_handle, num, out var pDetails))
				{
					if (pDetails.m_eResult == EResult.k_EResultOK)
					{
						CSteamID cSteamID = new CSteamID(pDetails.m_ulSteamIDOwner);
						switch (pDetails.m_eVisibility)
						{
						case ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityFriendsOnly:
							if (SteamFriends.GetFriendRelationship(cSteamID) != EFriendRelationship.k_EFriendRelationshipFriend)
							{
								continue;
							}
							break;
						case ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate:
							if (cSteamID != SteamUser.GetSteamID())
							{
								continue;
							}
							break;
						}
						validItemCount++;
						subscribedItem subscribedItem = default(subscribedItem);
						subscribedItem.title = pDetails.m_rgchTitle;
						subscribedItem.tags = pDetails.m_rgchTags;
						ref subscribedItem reference = ref subscribedItem;
						string text = itemsFolderPath;
						PublishedFileId_t nPublishedFileId = pDetails.m_nPublishedFileId;
						reference.path = text + "/" + nPublishedFileId.ToString() + "/";
						subscribedItem.authorId = cSteamID;
						subscribedItem.workshopItemId = pDetails.m_nPublishedFileId;
						if (SteamFriends.RequestUserInformation(subscribedItem.authorId, bRequireNameOnly: true))
						{
							Callback<PersonaStateChange_t>.Create(delegate(PersonaStateChange_t personaStateChange_t)
							{
								if (personaStateChange_t.m_nChangeFlags.HasFlag(EPersonaChange.k_EPersonaChangeNameFirstSet))
								{
									subscribedItem.author = SteamFriends.GetFriendPersonaName(subscribedItem.authorId);
									if (!string.IsNullOrEmpty(subscribedItem.author))
									{
										CompleteItem(subscribedItem);
									}
									else
									{
										SteamFriends.RequestUserInformation(subscribedItem.authorId, bRequireNameOnly: true);
									}
								}
							});
						}
						else
						{
							subscribedItem.author = SteamFriends.GetFriendPersonaName(subscribedItem.authorId);
							CompleteItem(subscribedItem);
						}
					}
				}
			}
		}).Set(hAPICall);
		void CompleteItem(subscribedItem item)
		{
			if (!subscribedItemsDictionary.ContainsKey(item.workshopItemId))
			{
				subscribedItemsDictionary.Add(item.workshopItemId, item);
			}
			isDownloading = subscribedItemsDictionary.Count < validItemCount;
		}
	}

	public bool CheckIsLastUploadSuccessful()
	{
		return isLastUploadSuccessful;
	}

	public bool CheckIsUploading()
	{
		return isUploading;
	}

	public bool CheckIsDownloading()
	{
		return isDownloading;
	}

	public List<subscribedItem> GetSubscribedItems()
	{
		return subscribedItemsDictionary.Values.ToList();
	}

	private string GetLevelNameFromScene()
	{
		return SceneMonitor.mgr.GetActiveSceneName() switch
		{
			"LvlEditor_food" => "Food", 
			"LvlEditor_shopping" => "Shopping", 
			"LvlEditor_tech" => "Tech", 
			"LvlEditor_followers" => "Followers", 
			"LvlEditor_indulgence" => "Indulgence", 
			"LvlEditor_exercise" => "Exercise", 
			"LvlEditor_career" => "Work", 
			"LvlEditor_money" => "Money", 
			"LvlEditor_dating" => "Dating", 
			"LvlEditor_pressure" => "Pressure", 
			"LvlEditor_time" => "Time", 
			"LvlEditor_mind" => "Mind", 
			"LvlEditor_space" => "Space", 
			"LvlEditor_nature" => "Nature", 
			"LvlEditor_meditation" => "Meditation", 
			"LvlEditor_stress" => "Stress", 
			"LvlEditor_desires" => "Desires", 
			"LvlEditor_past" => "Past", 
			"LvlEditor_future" => "Future", 
			"LvlEditor_setbacks" => "Setbacks", 
			_ => "New Day", 
		};
	}
}
