using ChessCrush;
using ChessCrush.Game;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChessActionView : MonoBehaviour
{
    public Text contentText;
    [SerializeField]
    private Button deleteButton;
    private ChessAction action;

    private ChessGameDirector chessGameDirector;

    private void Awake()
    {
        deleteButton.OnClickAsObservable().Subscribe(_ =>
        {
            var player = chessGameDirector.player;
            player.chessActions.Remove(action);
            player.actionsSubject.OnNext(player.chessActions);

            if(action.pieceId == 0)
                chessGameDirector.myLocalEnergy.Value += action.pieceType.GetNeedEnergy();
        });
    }

    private void Start()
    {
        chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
    }

    public void SetText(ChessAction action)
    {
        this.action = action;

        var sb = new StringBuilder();
        if (action.pieceId == 0)
            sb.Append("Create ");
        else
            sb.Append("Move ");

        sb.Append(action.pieceType.ToString());
        sb.Append($" to ({action.chessBoardVector.x}, {action.chessBoardVector.y})");
        contentText.text = sb.ToString();
    }
}
