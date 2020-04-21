using ChessCrush.Game;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ChessActionView : MonoBehaviour
{
    public Text contentText;
    [SerializeField]
    private Button deleteButton;

    public void SetText(ChessAction action)
    {
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
