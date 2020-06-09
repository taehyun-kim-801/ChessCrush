using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ChessCrush.Game
{
    public class Player: IDisposable
    {
        private ReactiveProperty<string> name = new ReactiveProperty<string>();
        private ReactiveProperty<bool> isWhite = new ReactiveProperty<bool>();
        public bool IsWhite { get { return isWhite.Value; } }
        private ReactiveProperty<int> hp = new ReactiveProperty<int>();
        private ReactiveProperty<int> energyPoint = new ReactiveProperty<int>();
        public bool IsMe { get; private set; }
        public List<ChessAction> chessActions = new List<ChessAction>();
        public Subject<List<ChessAction>> actionsSubject = new Subject<List<ChessAction>>();

        private ChessGameDirector chessGameDirector;

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

            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            if (isMe)
            {
                this.name.Subscribe(str => chessGameDirector.chessGameUI.myStatus.nameText.text = $"Player: {str}");
                //TODO: isWhite Subscribe
                hp.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.myStatus.hpText.text = $"{_} / 20";
                    chessGameDirector.chessGameUI.myStatus.hpBar.fillAmount = (float)_ / 20;
                });
                energyPoint.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.myStatus.energyText.text = $"{_} / 10";
                    chessGameDirector.chessGameUI.myStatus.energyBar.fillAmount = (float)_ / 10;
                });
                actionsSubject.Subscribe(list => chessGameDirector.chessGameUI.chessActionScroll.SetView(list));
                actionsSubject.Subscribe(list => chessGameDirector.chessGameObjects.SetExpectedAction(list));
            }
            else
            {
                this.name.Subscribe(str => chessGameDirector.chessGameUI.enemyStatus.nameText.text = $"Player: {str}");
                //TODO: isWhite Subscribe
                hp.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.enemyStatus.hpText.text = $"{_.ToString()} / 20";
                    chessGameDirector.chessGameUI.enemyStatus.hpBar.fillAmount = (float)_ / 20;
                });
                energyPoint.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.enemyStatus.energyText.text = $"{_.ToString()} / 10";
                    chessGameDirector.chessGameUI.enemyStatus.energyBar.fillAmount = (float)_ / 10;
                });
            }

            chessGameDirector.turnCount.Subscribe(value =>
            {
                if (value > 0) energyPoint.Value = Mathf.Clamp(energyPoint.Value + 1, 0, 10);
            }).AddTo(chessGameDirector);
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