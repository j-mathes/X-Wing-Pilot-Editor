using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace X_Wing_Pilot_Editor
{
    public enum Health
    {
        Alive,
        Captured,
        Dead
    }

    public enum Rank
    {
        Cadet,
        Officer,
        Lieutenant,
        Captain,
        Commander,
        General
    }

    public enum FlyableShipType
    {
        XWing,
        YWing,
        AWing,
        BWing
    }

    public enum HistoricMissionType
    {
        XWing,
        YWing,
        AWing,
        BWing,
        Bonus
    }

    public enum NPCShipType
    {                       // Kills offset Captures offset
        XWing,              // 1589-1590    1637-1638
        YWing,              // 1591-1592    1639-1640
        AWing,              // 1593-1594    1641-1642
        TIEFighter,         // 1595-1596    1643-1644
        TIEInterceptor,     // 1597-1598    1645-1646
        TIEBomber,          // 1599-1600    1647-1648
        GunBoat,            // 1601-1602    1649-1650
        Transport,          // 1603-1604    1651-1652
        Shuttle,            // 1605-1606    1653-1654
        Tug,                // 1607-1608    1655-1656
        Container,          // 1609-1610    1657-1658
        Freighter,          // 1611-1612    1659-1660
        Calamari,           // 1613-1614    1661-1662
        NebulonB,           // 1615-1616    1663-1664
        Corvette,           // 1617-1618    1665-1666
        StarDestroyer,      // 1619-1620    1667-1668
        TIEAdvanced,        // 1621-1622    1669-1670
        Mine1,              // 1623-1624    1671-1672
        Mine2,              // 1625-1626    1673-1674
        Mine3,              // 1627-1628    1675-1676
        Mine4,              // 1629-1630    1677-1678
        CommSat1,           // 1631-1632    1679-1680
        CommSat2,           // 1633-1634    1681-1682
        SpaceProbe,         // 1635-1636    1683-1684
    }

    public enum TourStatus
    {
        Inactive,
        Active,
        Incomplete,
        Complete
    }

    public enum KalidorCrescentLevel
    {
        None,
        KalidorCrescent,
        BronzeCluster,
        SilverTalons,
        SilverScimitar,
        GoldWings,
        DiamondEyes
    }
}
