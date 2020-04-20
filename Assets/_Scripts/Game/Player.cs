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
        private bool isMe;
        public ReactiveProperty<List<ChessAction>> chessActions = new ReactiveProperty<List<ChessAction>>();

        public Player()
        {
        }

        public Player(string name, bool isWhite, bool isMe)
        {
            this.name.Value = name;
            this.isWhite.Value = isWhite;
            hp.Value = 20;
            energyPoint.Value = 0;
            this.isMe = isMe;

            var gameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            if (isMe)
            {
                this.name.Subscribe(str => gameDirector.chessGameUI.myStatus.nameText.text = $"Player: {str}");
                //TODO: isWhite Subscribe
                hp.Subscribe(_ => gameDirector.chessGameUI.myStatus.hpBar.fillAmount = _ / 20);
                energyPoint.Subscribe(_ => gameDirector.chessGameUI.myStatus.energyBar.fillAmount = _ / 10);
                //TODO: chessActions Subscribe
            }
            else
            {
                this.name.Subscribe(str => gameDirector.chessGameUI.enemyStatus.nameText.text = $"Player:{str}");
                //TODO: isWhite Subscribe
                hp.Subscribe(_ => gameDirector.chessGameUI.enemyStatus.hpBar.fillAmount = _ / 20);
                energyPoint.Subscribe(_ => gameDirector.chessGameUI.enemyStatus.energyBar.fillAmount = _ / 10);
            }
        }

        public void Dispose()
        {
            name.Dispose();
            isWhite.Dispose();
            hp.Dispose();
            energyPoint.Dispose();
            chessActions.Dispose();
        }
    }
}