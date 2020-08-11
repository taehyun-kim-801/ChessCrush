using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class ButtonInformationPopup: MonoBehaviour
    {
        [SerializeField] private Text energyText;
        [SerializeField] private Text powerText;

        public void Use(PieceType pieceType)
        {
            gameObject.SetActive(true);
            energyText.text = pieceType.GetNeedEnergy().ToString();
            powerText.text = pieceType.GetPower().ToString();
        }
    }
}
