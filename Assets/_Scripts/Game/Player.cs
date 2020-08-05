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
        public ReactiveProperty<int> Hp { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> EnergyPoint { get; private set; } = new ReactiveProperty<int>();
        public bool IsMe { get; private set; }
        public List<ChessAction> chessActions = new List<ChessAction>();
        public Subject<List<ChessAction>> actionsSubject = new Subject<List<ChessAction>>();

        private IDisposable turnCountSubscribe;

        private ChessGameDirector chessGameDirector;

        public Player()
        {
        }

        public Player(string name, bool isWhite, bool isMe)
        {
            this.name.Value = name;
            this.isWhite.Value = isWhite;
            Hp.Value = 20;
            EnergyPoint.Value = 0;
            chessActions = new List<ChessAction>();
            this.IsMe = isMe;

            chessGameDirector = Director.instance.GetSubDirector<ChessGameDirector>();
            if (isMe)
            {
                this.name.Subscribe(str => chessGameDirector.chessGameUI.myStatus.nameText.text = $"Player: {str}");
                //TODO: isWhite Subscribe
                Hp.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.myStatus.hpText.text = $"{_} / 20";
                    chessGameDirector.chessGameUI.myStatus.hpBar.fillAmount = (float)_ / 20;
                });
                EnergyPoint.Where(_ => chessGameDirector.myLocalEnergy.Value is null).Subscribe(_ =>
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
                Hp.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.enemyStatus.hpText.text = $"{_.ToString()} / 20";
                    chessGameDirector.chessGameUI.enemyStatus.hpBar.fillAmount = (float)_ / 20;
                });
                EnergyPoint.Subscribe(_ =>
                {
                    chessGameDirector.chessGameUI.enemyStatus.energyText.text = $"{_.ToString()} / 10";
                    chessGameDirector.chessGameUI.enemyStatus.energyBar.fillAmount = (float)_ / 10;
                });
            }

            turnCountSubscribe = chessGameDirector.turnCount.Where(value => value > 0).Subscribe(_ => EnergyPoint.Value = Mathf.Clamp(EnergyPoint.Value + 1, 0, 10));
        }

        public void Initialize(int hp,int energy)
        {
            Hp.Value = hp;
            EnergyPoint.Value = energy;
        }

        public void DeleteAction(ChessAction chessAction, bool isRecursive = false)
        {
            chessActions.Remove(chessAction);

            if (chessAction.pieceId == 0)
                chessGameDirector.myLocalEnergy.Value += chessAction.pieceType.GetNeedEnergy();
            else
            {
                var chessBoard = chessGameDirector.chessGameObjects.chessBoard;
                var piece = chessBoard.GetChessPieceById(chessAction.pieceId);
                if (!(piece is null))
                {
                    ChessAction deleteAction = chessActions.Find(action => action.chessBoardVector.Equals(piece.chessBoardVector));
                    if (!(deleteAction is null))
                        DeleteAction(deleteAction, true);
                }
            }
            if(!isRecursive)
                actionsSubject.OnNext(chessActions);
            chessGameDirector.expectedActionDeleteSubject.OnNext(chessAction.pieceId);
        }

        public void Dispose()
        {
            name.Dispose();
            isWhite.Dispose();
            Hp.Dispose();
            EnergyPoint.Dispose();
            actionsSubject.Dispose();
            turnCountSubscribe.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}