﻿using ChessCrush.Game;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ChessCrush.UI
{
    public class AbleMoveSquare: ChessBoardObject
    {
        private PieceType spawnType;
        private Button button;
        private ChessBoard chessBoard;
        public static ReactiveProperty<bool> haveToAppearProperty = new ReactiveProperty<bool>();
        private IDisposable haveToAppearPropertyCallback;

        private void Awake()
        {
            base.Awake();
            button = gameObject.GetComponent<Button>();
            chessBoard = Director.instance.chessBoard;
            button.OnClickAsObservable().Subscribe(_ => SubscribeAbleMoveSquare()).AddTo(gameObject);
        }

        private void SubscribeAbleMoveSquare()
        {
            var piece = ChessPiece.UseWithComponent(chessBoardPosition.x, chessBoardPosition.y, spawnType);
            
            if(spawnType == default)
            {
                piece.MoveTo(chessBoardPosition.x, chessBoardPosition.y);
            }
            else
            {
                chessBoard.AddChessPiece(piece);
            }

            haveToAppearProperty.Value = false;
        }

        private void Initialize(int x, int y, PieceType spawnType)
        {
            base.Initialize(x, y);
            this.spawnType = spawnType;
            haveToAppearProperty.Value = true;
            haveToAppearPropertyCallback = haveToAppearProperty.Subscribe(value =>
            {
                if(!value)
                    Destroy();
            });
        }

        private void Destroy()
        {
            DisableMoveSquare.haveToAppearProperty.Value = false;
            gameObject.SetActive(false);
            haveToAppearPropertyCallback.Dispose();
        }

        public static AbleMoveSquare UseWithComponent(ChessBoardVector boardVector, PieceType spawnType = default)
        {
            var result = Director.instance.uiObjectPool.Use(nameof(AbleMoveSquare)).GetComponent<AbleMoveSquare>();
            result.Initialize(boardVector.x, boardVector.y, spawnType);
            return result;
        }
    }
}