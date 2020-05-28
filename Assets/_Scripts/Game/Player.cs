using System;
using System.Collections.Generic;
using UniRx;

namespace ChessCrush.Game
{
    public class Player: IDisposable
    {
        private ReactiveProperty<string> name = new ReactiveProperty<string>();
        private ReactiveProperty<bool> isWhite = new ReactiveProperty<bool>();
        private ReactiveProperty<int> hp = new ReactiveProperty<int>();
        private ReactiveProperty<int> energyPoint = new ReactiveProperty<int>();
        public bool IsMe { get; private set; }
        public List<ChessAction> chessActions = new List<ChessAction>();
        public Subject<List<ChessAction>> actionsSubject = new Subject<List<ChessAction>>();

        public Player()
        {
        }

        public Player(string name, bool isWhite, bool isMe)
        {
            this.name.Value = name;
            this.isWhite.Value = isWhite;
            hp.Value = 20;
            energyPoint.Value = 0;
            chessActions = new List<ChessAction>();
            this.IsMe = isMe;

            var gameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            if (isMe)
            {
                this.name.Subscribe(str => gameDirector.chessGameUI.myStatus.nameText.text = $"Player: {str}");
                //TODO: isWhite Subscribe
                hp.Subscribe(_ =>
                {
                    gameDirector.chessGameUI.myStatus.hpText.text = $"{_} / 20";
                    gameDirector.chessGameUI.myStatus.hpBar.fillAmount = _ / 20;
                });
                energyPoint.Subscribe(_ =>
                {
                    gameDirector.chessGameUI.myStatus.energyText.text = $"{_} / 10";
                    gameDirector.chessGameUI.myStatus.energyBar.fillAmount = _ / 10;
                });
                actionsSubject.Subscribe(list => gameDirector.chessGameUI.chessActionScroll.SetView(list));
                actionsSubject.Subscribe(list => gameDirector.chessGameObjects.SetExpectedAction(list));
            }
            else
            {
                this.name.Subscribe(str => gameDirector.chessGameUI.enemyStatus.nameText.text = $"Player: {str}");
                //TODO: isWhite Subscribe
                hp.Subscribe(_ =>
                {
                    gameDirector.chessGameUI.enemyStatus.hpText.text = $"{_} / 20";
                    gameDirector.chessGameUI.enemyStatus.hpBar.fillAmount = _ / 20;
                });
                energyPoint.Subscribe(_ =>
                {
                    gameDirector.chessGameUI.enemyStatus.energyText.text = $"{_} / 10";
                    gameDirector.chessGameUI.enemyStatus.energyBar.fillAmount = _ / 10;
                });
            }
        }

        public void Dispose()
        {
            name.Dispose();
            isWhite.Dispose();
            hp.Dispose();
            energyPoint.Dispose();
            actionsSubject.Dispose();
        }
    }
}