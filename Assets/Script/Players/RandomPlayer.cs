using System;
using System.Collections;
using System.Collections.Generic;
using Help;
using UnityEngine;
using Random = System.Random;

namespace General
{
    public class RandomPlayer : Player
    {
        
        public override IEnumerator MakeDecision()
        {
            yield return new WaitForSeconds(1f);
            var random = new Random();
            var validMoves = Manager.game.GetValidMoves();
            chosenMove = validMoves[random.Next(0, validMoves.Count)];
            isReady = true;
            yield return null;
        }

    }
}