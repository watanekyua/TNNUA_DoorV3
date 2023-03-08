// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// //using com.rfilkov.kinect;

// public class UserImagePos : MonoBehaviour
// {
//     public Canvas targetCanvas;
//     public BackgroundRemovalManager bgrManager;
//     public float distAmp = 1.0f;
//     private KinectManager kinectManager = null;

//     int pelvisBoneIndex = 0;

//     void Start()
//     {
//         kinectManager = KinectManager.Instance;
//         targetCanvas = GetComponent<Canvas>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (kinectManager && kinectManager.IsInitialized())
//         {
            
//             if (kinectManager.IsUserDetected(bgrManager.playerIndex))
//             {
//                 ulong userId = kinectManager.GetUserIdByIndex(bgrManager.playerIndex);

//                 if (kinectManager.IsJointTracked(userId, pelvisBoneIndex))
//                 {
//                     Vector3 posJoint = kinectManager.GetJointPosition(userId, pelvisBoneIndex);

//                     targetCanvas.planeDistance = posJoint.z * distAmp;

//                     Debug.Log($"U ({bgrManager.playerIndex}) POS : {posJoint}");
//                 }
//             }
//         }
//     }
// }
