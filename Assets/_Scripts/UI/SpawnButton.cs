using ChessCrush.Game;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class SpawnButton: MonoBehaviour
    {
        private Button button;
        private Image buttonImage;
        [SerializeField]
        private Image chessImage;
        public PieceType pieceSpawnType;

        private ChessGameDirector chessGameDirector;
        private ResourceDirector resourceDirector;
        private ButtonInformationPopup buttonInformationPopup;

        private void Awake()
        {
            button = GetComponent<Button>();
            buttonImage = gameObject.GetComponent<Image>();

            button.OnPointerEnterAsObservable().Subscribe(_ => buttonInformationPopup.Use(pieceSpawnType)).AddTo(gameObject);
            Observable.EveryUpdate().SkipUntil(button.OnPointerEnterAsObservable()).TakeUntil(button.OnPointerExitAsObservable()).RepeatUntilDestroy(this)
                .Select(_ => Input.mousePosition).Subscribe(position => buttonInformationPopup.transform.position = position);
            button.OnPointerExitAsObservable().Subscribe(_ => buttonInformationPopup.gameObject.SetActive(false)).AddTo(gameObject);

            button.OnClickAsObservable().Subscribe(_ => SubscribeButton());
        }

        private void Start()
        {
            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            resourceDirector = Director.instance.GetSubDirector<ResourceDirector>();
            buttonInformationPopup = chessGameDirector.chessGameUI.ButtonInformationPopup;

            chessGameDirector.gameReadyEvents += () =>
              {
                  if (chessGameDirector.player.IsWhite)
                  {
                      buttonImage.color = Color.black;
                      chessImage.sprite = resourceDirector.GetChessSprite(pieceSpawnType, true);
                  }
                  else
                  {
                      buttonImage.color = Color.white;
                      chessImage.sprite = resourceDirector.GetChessSprite(pieceSpawnType, false);
                  }
              };

            chessGameDirector.myLocalEnergy.Where(value => !(value is null)).Subscribe(energy => button.enabled = pieceSpawnType.GetNeedEnergy() <= energy).AddTo(chessGameDirector);
        }

        private void SubscribeButton()
        {
            //기존에 있던 버튼들 제거
            AbleMoveSquare.haveToAppearProperty.Value = false;
            DisableMoveSquare.haveToAppearProperty.Value = false;
            AbleMoveSquare.haveToAppearProperty.Value = true;
            DisableMoveSquare.haveToAppearProperty.Value = true;

            for (int i = 0; i < 8; i++)
            {
                if (chessGameDirector.chessGameObjects.chessBoard.AnybodyIn(i, 0, true))
                {
                    var square = DisableMoveSquare.UseWithComponent(new ChessBoardVector(i, 0));
                }
                else
                {
                    var square = AbleMoveSquare.UseWithComponent(0, new ChessBoardVector(i, 0), pieceSpawnType);
                }
            }
        }
    }
}