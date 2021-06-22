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
        public string spriteName;

        public SetupData(int lvl, SetupType setupType, CannonType cannonType, string spriteName)
        {
            this.lvl = lvl;
            this.setupType = setupType;
            this.cannonType = cannonType;
            this.spriteName = spriteName;
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


