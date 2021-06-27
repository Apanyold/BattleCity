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
        public DataType dataType;
        public CannonType cannonType;

        public SetupData(int lvl, DataType dataType, CannonType cannonType)
        {
            this.lvl = lvl;
            this.dataType = dataType;
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


