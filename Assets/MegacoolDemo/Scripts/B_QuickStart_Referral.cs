using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

public class B_QuickStart_Referral : MonoBehaviour {

    public GameObject rewardPopup;
    public Button referAFriendButton;
    public Button exitPopupButton;
    public Button refreshButton;

    void Start() {


        // OnMegacoolEvents
        // Recieves a list of all megacool events as they occur:
        // LinkClicked, RecievedShareOpened, SentShareOpened
        Megacool.OnMegacoolEvents += (List<MegacoolEvent> events) => {
            foreach (MegacoolEvent e in events) {
                Debug.Log (e.Type + " event recieved");
            }
        };

        // LinkClicked
        // The app has been opened from a link click, send the user instantly to
        // the right scene if the URL path exists
        Megacool.OnLinkClicked += (MegacoolEvent e) => {
            Debug.Log("Event LinkClicked");
            Debug.Log("Url: " + e.Data["url"]);
            Debug.Log("Invited by: " + ((MegacoolReferralCode)e.Data["referralCode"]).InviteId);
        };

        // ReceivedShareOpened
        // This device has received a share to the app, including a share object
        Megacool.OnReceivedShareOpened += (MegacoolEvent e) => {
            Debug.Log("Event ReceivedShareOpened");
            Debug.Log(e);
            string invitedById = e.Share.ReferralCode.InviteId;

            if(e.FirstSession){
                //First session means it's a new install
                Debug.Log("WelcomeText " + e.Share.Data["welcomeText"]);
                //Add your friend to the friends list - based on InviteId
                Debug.Log("New install for this device, invited by: "+ invitedById);
            }
            else{
                //The app is already installed
                string welcomeText = "";
                if (e.Share.Data.TryGetValue("welcomeText", out welcomeText)) {
                    Debug.Log("Welcome text: " + welcomeText);
                }
                Debug.Log("Open app, invited by: "+ invitedById);
            }
        };

        // SentShareOpened
        // A Friend has received a share from your device
        Megacool.OnSentShareOpened += (MegacoolEvent e) => {
            Debug.Log("Event SentShareOpened");
            string sentInviteToId = (string) e.Data["receiverUserId"];
            if(e.FirstSession){
                //First session means your friend just installed the app
                Debug.Log("First session for friend with id: "+ sentInviteToId);
            }
            else{
                //The app was already installed and was opened from a link
                Debug.Log("New session for friend with id: "+ sentInviteToId);
            }
            GiveReferenceReward();
        };


        // Refer a friend button
        Megacool.Instance.SharingText = "Referral example. Try out this cool game!";
        referAFriendButton.onClick.AddListener(ReferAFriend);

        // Reward popup exit button
        exitPopupButton.onClick.AddListener(() => {
            rewardPopup.SetActive(false);
        });

        // Refresh button to check for an opened referral
        refreshButton.onClick.AddListener(Refresh);
    }
        
    // Gives the player a reward for sending an opened referral
    void GiveReferenceReward() {
        Debug.Log ("Give Reference Reward");
        rewardPopup.SetActive(true);
    }

    // Creates and sends a referral link to a friend
    void ReferAFriend() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add ("welcomeText", "Nick has invited you!");
        MegacoolShare share = new MegacoolShare("referral", data);
        Megacool.Instance.Share(new MegacoolShareConfig {
            Share = share,
            RecordingId = "_guaranteedToBeUniqueRecordingId",
        });
    }

    // Refreshes events to check for an opened referral
    void Refresh() {
        Megacool.Instance.GetShares(shares => {});
    }
}
