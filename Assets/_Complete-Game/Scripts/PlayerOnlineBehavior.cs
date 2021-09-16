using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Completed;


public  class PlayerOnlineBehavior : OnlineBehavior
{
    Player m_player = null;
    public void Start()
    {
        Init();
        m_player = GetComponent<Player>();
    }
    float lastx = 0;
    float lasty = 0;
    public override bool NeedSync()
    {
        return lastx != m_player.transform.position.x
        || lasty != m_player.transform.position.y;
    }
     public override  void Write(BinaryWriter w)
    {
        w.Write(m_player.Food); 
        w.Write(m_player.transform.position.x); 
        w.Write(m_player.transform.position.y);
        w.Write(GameManager.instance.playersTurn);
        lastx = m_player.transform.position.x;
        lasty = m_player.transform.position.y;

    }

    public override  void Read(BinaryReader r)
    {
        m_player.Food = r.ReadInt32();
        float x = r.ReadSingle();
        float y = r.ReadSingle();
        m_player.transform.position = new Vector3(x, y);
        GameManager.instance.playersTurn = r.ReadInt32();
    }
  }
