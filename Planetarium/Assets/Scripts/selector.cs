﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selector : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	private Transform _LeftHandAnchor;

	[SerializeField]
	private Transform _RightHandAnchor;

	[SerializeField]
	private Transform _CenterEyeAnchor;

	[SerializeField]
	private float _MaxDistance = 100.0f;
	private float _MinDistance = 0.0f;

	[SerializeField]

	private LineRenderer _LaserPointerRenderer;

	private Transform Pointer {
        get {
            // 現在アクティブなコントローラーを取得
            var controller = OVRInput.GetActiveController();
            if (controller == OVRInput.Controller.RTrackedRemote) {
                return _RightHandAnchor;
            } else if (controller == OVRInput.Controller.LTrackedRemote) {
                return _LeftHandAnchor;
            }
            // どちらも取れなければ目の間からビームが出る
            return _CenterEyeAnchor;
        }
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var pointer = Pointer;
        if (pointer == null || _LaserPointerRenderer == null)
        {
            return;
        }
        // コントローラーのトリガー
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
            // コントローラー位置からRayを飛ばす
            Ray pointerRay = new Ray(pointer.position, pointer.forward);
            // レーザーの起点
            _LaserPointerRenderer.SetPosition(0, pointerRay.origin);

            RaycastHit hitInfo;
            if (Physics.Raycast(pointerRay, out hitInfo, _MaxDistance))
            {
                // Rayがヒットしたらそこまで
                _LaserPointerRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                // Rayがヒットしなかったら向いている方向にMaxDistance伸ばす
                _LaserPointerRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * _MaxDistance);
            }
        }
        else
        {
            // コントローラー位置からRayを飛ばす
            Ray pointerRay = new Ray(pointer.position, pointer.forward);
            // レーザーの起点
            _LaserPointerRenderer.SetPosition(0, pointerRay.origin);
            // コントローラーのトリガー無しの場合、Rayが向いている方向にMinDistance（０）
            _LaserPointerRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * _MinDistance);
        }
	}
}
