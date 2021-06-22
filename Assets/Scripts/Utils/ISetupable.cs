using Didenko.BattleCity.Behaviors;

namespace Didenko.BattleCity.Utils
{
    public interface ISetupable
    {
        void Setup(SetupData setupData);
        void InitSetup();
    }

    public struct SetupData
    {
        public int lvl;
        public SetupType setupType;
        public CannonType cannonType;

        public SetupData(int lvl, SetupType setupType, CannonType cannonType)
        {
            this.lvl = lvl;
            this.setupType = setupType;
            this.cannonType = cannonType;
        }

        public SetupData(SetupData setupData)
        {
            this = setupData;        
        }
    }

    public enum SetupType
    {
        Hull,
        Cannon,
        Tower
    }
}


