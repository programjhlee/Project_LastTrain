using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MoveTutorialStep" , menuName = "Create Tutorial File / MoveTutorial")]
public class MoveTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _uiAnnounceStrategy;
    [SerializeField] Sprite _moveKeySprite;
    UI_Announce _uiAnnounce;
    public override void Bind(TutorialSystem system)
    {
        _uiAnnounce = UIManager.Instance.ShowUIAt<UI_Announce>(new Vector2(0,300f));
        _uiAnnounce.Init();
        _uiAnnounce.SetUIStrategy(_uiAnnounceStrategy);
        _uiAnnounce.SetQuestSprite(_moveKeySprite);
    }

    public override IEnumerator Run()
    {
        float curDistance = 0;
        float movementTutorialClearDistance = 20f;
       
        while (curDistance < movementTutorialClearDistance)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            _uiAnnounce.SetAnnounceText($"TO MOVE \r\n<size=25> DISTANCE {curDistance:F2} / {movementTutorialClearDistance:F2}M</size>");
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                curDistance += 0.3f;
                curDistance = Mathf.Min(curDistance, movementTutorialClearDistance);
            }
            yield return null;
        }
        _uiAnnounce.SetAnnounceText($"TO MOVE \r\n<size=25> DISTANCE {curDistance:F2} / {movementTutorialClearDistance:F2}M</size>");
        _uiAnnounce.QuestClear();
    }

    public override void Release()
    {
        _uiAnnounce.Hide();
    }
}
