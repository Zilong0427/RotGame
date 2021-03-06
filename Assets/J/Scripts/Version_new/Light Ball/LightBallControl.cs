﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBallControl : MonoBehaviour {
    public GameEnum.Type_Color TypeColor;
    public float SelfLastTime;
    [Range(0,0.8f)]
    public float s;

    public CircleParmeter _CircleParmeter;
    Animator m_Animator;
	AudioSource audio;
	public AudioClip[] sound=new AudioClip[2];

    public GameObject Explo;
	public GameObject particle;
    float lasttime;
	// Use this for initialization
	void Start () {
        m_Animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource> ();
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        if(_CircleParmeter==null){
            _CircleParmeter=transform.GetComponentInParent<CircleParmeter>();

        }

        lasttime += Time.deltaTime;
        s = 0.8f - (lasttime / SelfLastTime) * 0.8f;
        transform.localScale = new Vector3(s, s, s);
        if(lasttime>=SelfLastTime)
        {
            _CircleParmeter.LimitNumber++;
            Init();
            if (Explo != null)
            {
                GameObject tempEx = JObjectPool._InstanceJObjectPool.GetGameObject(Explo.name, transform.position);
                JObjectPool._InstanceJObjectPool.DelayRecovery(tempEx, 1.5f);
            }
            JObjectPool._InstanceJObjectPool.Recovery(this.gameObject,Vector3.zero);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        FlyingObjControl _FlyingObjControl = other.GetComponent<FlyingObjControl>();
        if (_FlyingObjControl)
        {
            bool isSame = _FlyingObjControl.TypeColor == TypeColor;
            _FlyingObjControl.GetHit(isSame);
            GetHit(isSame);
        }
    }
    void GetHit(bool b)
    {
        if (b)
        {
            lasttime = 0;
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			audio.clip = sound [0];
			audio.Play ();
        }
        //如果碰撞的顏色不一樣的話，爆炸並且回收自己
        else{
			audio.clip = sound [1];
			audio.Play ();
            _CircleParmeter.LimitNumber++;
            Init();
			JObjectPool._InstanceJObjectPool.GetGameObject (Explo.name, transform.position);
            JObjectPool._InstanceJObjectPool.Recovery(this.gameObject,Vector3.zero);

        }
    }
    public void Init()
    {
        lasttime = 0;
        _CircleParmeter=null;
        s=0.8f;
    }
}
