﻿using System.Collections;
using Scripts.Characters;
using Scripts.Worlds;

namespace Scripts.Turns.Actions
{
    public class MoveAction : TurnAction
    {
        private readonly Character _character;
        private readonly WorldTile _tile;

        public MoveAction(Character character, WorldTile tile)
        {
            _character = character;
            _tile = tile;
        }

        protected override IEnumerator ActInternal()
        {
            return _character.MoveRoutine(_tile);
        }
    }
}
