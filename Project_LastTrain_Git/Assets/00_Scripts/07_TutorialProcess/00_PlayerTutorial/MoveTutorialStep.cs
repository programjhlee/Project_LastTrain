using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MoveTutorialStep" , menuName = "Create Tutorial File / MoveTutorial")]
public class MoveTutorialStep : TutorialStep
{
    UI_Announce _uiAnnounce;
    public override void Bind(TutorialSystem system)
    {
        _uiAnnounce = UIManager.Instance.ShowUIAt<UI_Announce>(new Vector2(0,300f));
        _uiAnnounce.Init();
    }

    public override IEnumerator Run()
    {
        float curDistance = 0;
        float movementTutorialClearDistance = 20f;
       
        _uiAnnounce.SetQuestText($"캐릭터 움직이기 \r\n<size=60>거리 {curDistance:F2} / {movementTutorialClearDistance:F2}M</size>");
        while (curDistance < movementTutorialClearDistance)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                curDistance += 0.3f;
                curDistance = Mathf.Min(curDistance, movementTutorialClearDistance);
                _uiAnnounce.SetQuestText($"캐릭터 움직이기 \r\n<size=60>거리 {curDistance:F2} / {movementTutorialClearDistance:F2}M</size>");
            }
            yield return null;
        }
        _uiAnnounce.QuestClear();
    }

    public override void Release()
    {
        _uiAnnounce.Hide();
    }
}
