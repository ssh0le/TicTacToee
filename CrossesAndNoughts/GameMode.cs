using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossesAndNoughts
{
    class GameMode
    {
        const RivalType defaultRivalMode = RivalType.AI;
        public RivalType RivalMode;
        const SignType defaultSignMode = SignType.Cross;
        public SignType SignMode;
        const FirstMoveType defaultFirstMoveMode = FirstMoveType.Host;
        public FirstMoveType FirstMoveMode;

        public GameMode()
        {
            RivalMode = defaultRivalMode;
            SignMode = defaultSignMode;
            FirstMoveMode = defaultFirstMoveMode;
        }

        public enum RivalType
        {
            AI,
            VersusPlayer,
        }

        public void ShiftRivalMode() => RivalMode = RivalMode == RivalType.AI ? RivalType.VersusPlayer : RivalType.AI;

        public enum SignType
        {
            Cross,
            Nought,
            Random
        }

        public void ShiftSignMode()
        {
            switch (SignMode)
            {
                case SignType.Cross: SignMode = SignType.Nought; break;
                case SignType.Nought: SignMode = SignType.Random; break;
                case SignType.Random: SignMode = SignType.Cross; break;
            }
        }

        public enum FirstMoveType
        {
            Host,
            NotHost,
            Random
        }

        public void ShiftFirstMoveMode()
        {
            switch (FirstMoveMode)
            {
                case FirstMoveType.Host: FirstMoveMode = FirstMoveType.NotHost; break;
                case FirstMoveType.NotHost: FirstMoveMode = FirstMoveType.Random; break;
                case FirstMoveType.Random: FirstMoveMode = FirstMoveType.Host; break;
            }
        }
    }
}
