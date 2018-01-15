using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGame.Models.Machine
{
    public class MachineConfig
    {
        public PRGame PRGame { get; set; }
        public List<string> PRFlippers { get; set; }
        public List<string> PRBumpers { get; set; }
        public SwitchBase PRSwitches { get; set; }
        //public PRSwitches PRSwitches { get; set; }
        public PRCoils PRCoils { get; set; }
        public PRLamps PRLamps { get; set; }
        public PRBallSave PRBallSave { get; set; }
    }

    public class PRBallSave
    {
        public List<string> pulseCoils { get; set; }
        public ResetSwitches resetSwitches { get; set; }
        public StopSwitches stopSwitches { get; set; }
    }    

    public class PRGame
    {
        public string machineType { get; set; }
        public string numBalls { get; set; }
    }

    public class SwitchBase
    {
        public List<SwitchBase> Switches { get; set; }        
    }

    public class Switch
    {
        public string Number { get; set; }
        public string Tags { get; set; }
        public string Type { get; set; }
    }

    public class FlipperLwR
    {
        public string number { get; set; }
    }

    public class FlipperLwL
    {
        public string number { get; set; }
    }

    public class FlipperUpR
    {
        public string number { get; set; }
    }

    public class FlipperUpL
    {
        public string number { get; set; }
    }

    public class Exit
    {
        public string number { get; set; }
    }

    public class Down
    {
        public string number { get; set; }
    }

    public class Up
    {
        public string number { get; set; }
    }

    public class Enter
    {
        public string number { get; set; }
    }

    public class FireL
    {
        public string number { get; set; }
    }

    public class FireR
    {
        public string number { get; set; }
    }

    public class StartButton
    {
        public string number { get; set; }
    }

    public class Tilt
    {
        public string number { get; set; }
    }

    public class ShooterL
    {
        public string number { get; set; }
    }

    public class OutlaneL
    {
        public string number { get; set; }
    }

    public class InlaneL
    {
        public string number { get; set; }
    }

    public class ThreeBankTargets
    {
        public string number { get; set; }
    }

    public class SlamTilt
    {
        public string number { get; set; }
    }

    public class CoinDoor
    {
        public string number { get; set; }
    }

    public class TicketDispenser
    {
        public string number { get; set; }
    }

    public class AlwaysClosed
    {
        public string number { get; set; }
    }

    public class RightTopPost
    {
        public string number { get; set; }
    }

    public class CaptiveBall1
    {
        public string number { get; set; }
    }

    public class Mystery
    {
        public string number { get; set; }
    }

    public class BallOnMagnet
    {
        public string number { get; set; }
    }

    public class BuyIn
    {
        public string number { get; set; }
    }

    public class LeftRampEnter
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class LeftRollover
    {
        public string number { get; set; }
    }

    public class InlaneR
    {
        public string number { get; set; }
    }

    public class TopCenterRollover
    {
        public string number { get; set; }
    }

    public class LeftScorePost
    {
        public string number { get; set; }
    }

    public class SubwayEnter1
    {
        public string number { get; set; }
    }

    public class SubwayEnter2
    {
        public string number { get; set; }
    }

    public class ShooterR
    {
        public string number { get; set; }
        public string tags { get; set; }
    }

    public class OutlaneR
    {
        public string number { get; set; }
    }

    public class InlaneFarR
    {
        public string number { get; set; }
    }

    public class SuperGame
    {
        public string number { get; set; }
    }

    public class S45
    {
        public string number { get; set; }
    }

    public class S46
    {
        public string number { get; set; }
    }

    public class S47
    {
        public string number { get; set; }
    }

    public class S48
    {
        public string number { get; set; }
    }

    public class SlingL
    {
        public string number { get; set; }
    }

    public class SlingR
    {
        public string number { get; set; }
    }

    public class CaptiveBall2
    {
        public string number { get; set; }
    }

    public class DropTargetJ
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class DropTargetU
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class DropTargetD
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class DropTargetG
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class DropTargetE
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class GlobePosition1
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class CraneRelease
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class LeftRampToLock
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class LeftRampExit
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class RightRampEnter
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class CenterRampExit
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class LeftRampEnterAlt
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class CaptiveBall3
    {
        public string number { get; set; }
    }

    public class MagnetOverRing
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class TopRightOpto
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class PopperL
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class PopperR
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class TopRampExit
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class RightRampExit
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class GlobePosition2
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class S78
    {
        public string number { get; set; }
    }

    public class Trough1
    {
        public string number { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
    }

    public class Trough2
    {
        public string number { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
    }

    public class Trough3
    {
        public string number { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
    }

    public class Trough4
    {
        public string number { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
    }

    public class Trough5
    {
        public string number { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
    }

    public class Trough6
    {
        public string number { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
    }

    public class TroughTop
    {
        public string number { get; set; }
        public string type { get; set; }
    }

    public class S88
    {
        public string number { get; set; }
    }

    public class PRSwitches
    {
        public FlipperLwR flipperLwR { get; set; }
        public FlipperLwL flipperLwL { get; set; }
        public FlipperUpR flipperUpR { get; set; }
        public FlipperUpL flipperUpL { get; set; }
        public Exit exit { get; set; }
        public Down down { get; set; }
        public Up up { get; set; }
        public Enter enter { get; set; }
        public FireL fireL { get; set; }
        public FireR fireR { get; set; }
        public StartButton startButton { get; set; }
        public Tilt tilt { get; set; }
        public ShooterL shooterL { get; set; }
        public OutlaneL outlaneL { get; set; }
        public InlaneL inlaneL { get; set; }
        public ThreeBankTargets threeBankTargets { get; set; }
        public SlamTilt slamTilt { get; set; }
        public CoinDoor coinDoor { get; set; }
        public TicketDispenser ticketDispenser { get; set; }
        public AlwaysClosed alwaysClosed { get; set; }
        public RightTopPost rightTopPost { get; set; }
        public CaptiveBall1 captiveBall1 { get; set; }
        public Mystery mystery { get; set; }
        public BallOnMagnet ballOnMagnet { get; set; }
        public BuyIn buyIn { get; set; }
        public LeftRampEnter leftRampEnter { get; set; }
        public LeftRollover leftRollover { get; set; }
        public InlaneR inlaneR { get; set; }
        public TopCenterRollover topCenterRollover { get; set; }
        public LeftScorePost leftScorePost { get; set; }
        public SubwayEnter1 subwayEnter1 { get; set; }
        public SubwayEnter2 subwayEnter2 { get; set; }
        public ShooterR shooterR { get; set; }
        public OutlaneR outlaneR { get; set; }
        public InlaneFarR inlaneFarR { get; set; }
        public SuperGame superGame { get; set; }
        public S45 s45 { get; set; }
        public S46 s46 { get; set; }
        public S47 s47 { get; set; }
        public S48 s48 { get; set; }
        public SlingL slingL { get; set; }
        public SlingR slingR { get; set; }
        public CaptiveBall2 captiveBall2 { get; set; }
        public DropTargetJ dropTargetJ { get; set; }
        public DropTargetU dropTargetU { get; set; }
        public DropTargetD dropTargetD { get; set; }
        public DropTargetG dropTargetG { get; set; }
        public DropTargetE dropTargetE { get; set; }
        public GlobePosition1 globePosition1 { get; set; }
        public CraneRelease craneRelease { get; set; }
        public LeftRampToLock leftRampToLock { get; set; }
        public LeftRampExit leftRampExit { get; set; }
        public RightRampEnter rightRampEnter { get; set; }
        public CenterRampExit centerRampExit { get; set; }
        public LeftRampEnterAlt leftRampEnterAlt { get; set; }
        public CaptiveBall3 captiveBall3 { get; set; }
        public MagnetOverRing magnetOverRing { get; set; }
        public TopRightOpto topRightOpto { get; set; }
        public PopperL popperL { get; set; }
        public PopperR popperR { get; set; }
        public TopRampExit topRampExit { get; set; }
        public RightRampExit rightRampExit { get; set; }
        public GlobePosition2 globePosition2 { get; set; }
        public S78 s78 { get; set; }
        public Trough1 trough1 { get; set; }
        public Trough2 trough2 { get; set; }
        public Trough3 trough3 { get; set; }
        public Trough4 trough4 { get; set; }
        public Trough5 trough5 { get; set; }
        public Trough6 trough6 { get; set; }
        public TroughTop troughTop { get; set; }
        public S88 s88 { get; set; }
    }

    public class FlipperLwRMain
    {
        public string number { get; set; }
    }

    public class FlipperLwRHold
    {
        public string number { get; set; }
    }

    public class FlipperLwLMain
    {
        public string number { get; set; }
    }

    public class FlipperLwLHold
    {
        public string number { get; set; }
    }

    public class FlipperUpRMain
    {
        public string number { get; set; }
    }

    public class FlipperUpRHold
    {
        public string number { get; set; }
    }

    public class FlipperUpLMain
    {
        public string number { get; set; }
    }

    public class FlipperUpLHold
    {
        public string number { get; set; }
    }

    public class CraneMagnet
    {
        public string number { get; set; }
    }

    public class PopperL2
    {
        public string number { get; set; }
    }

    public class PopperR2
    {
        public string number { get; set; }
    }

    public class Crane
    {
        public string number { get; set; }
    }

    public class ResetDropTarget
    {
        public string number { get; set; }
    }

    public class GlobeMotor
    {
        public string number { get; set; }
    }

    public class Knocker
    {
        public string number { get; set; }
    }

    public class ShooterR2
    {
        public string number { get; set; }
    }

    public class ShooterL2
    {
        public string number { get; set; }
    }

    public class TripDropTarget
    {
        public string number { get; set; }
    }

    public class Diverter
    {
        public string number { get; set; }
    }

    public class Trough
    {
        public string number { get; set; }
    }

    public class SlingL2
    {
        public string number { get; set; }
    }

    public class SlingR2
    {
        public string number { get; set; }
    }

    public class FlasherFire
    {
        public string number { get; set; }
    }

    public class FlasherFear
    {
        public string number { get; set; }
    }

    public class FlasherDeath
    {
        public string number { get; set; }
    }

    public class FlasherMortis
    {
        public string number { get; set; }
    }

    public class FlasherPursuitL
    {
        public string number { get; set; }
    }

    public class FlasherPursuitR
    {
        public string number { get; set; }
    }

    public class FlasherBlackout
    {
        public string number { get; set; }
    }

    public class FlasherCursedEarth
    {
        public string number { get; set; }
    }

    public class FlashersLowerLeft
    {
        public string number { get; set; }
    }

    public class FlasherGlobe
    {
        public string number { get; set; }
    }

    public class FlashersRtRamp
    {
        public string number { get; set; }
    }

    public class FlashersInsert
    {
        public string number { get; set; }
    }

    public class PRCoils
    {
        public FlipperLwRMain flipperLwRMain { get; set; }
        public FlipperLwRHold flipperLwRHold { get; set; }
        public FlipperLwLMain flipperLwLMain { get; set; }
        public FlipperLwLHold flipperLwLHold { get; set; }
        public FlipperUpRMain flipperUpRMain { get; set; }
        public FlipperUpRHold flipperUpRHold { get; set; }
        public FlipperUpLMain flipperUpLMain { get; set; }
        public FlipperUpLHold flipperUpLHold { get; set; }
        public CraneMagnet craneMagnet { get; set; }
        public PopperL2 popperL { get; set; }
        public PopperR2 popperR { get; set; }
        public Crane crane { get; set; }
        public ResetDropTarget resetDropTarget { get; set; }
        public GlobeMotor globeMotor { get; set; }
        public Knocker knocker { get; set; }
        public ShooterR2 shooterR { get; set; }
        public ShooterL2 shooterL { get; set; }
        public TripDropTarget tripDropTarget { get; set; }
        public Diverter diverter { get; set; }
        public Trough trough { get; set; }
        public SlingL2 slingL { get; set; }
        public SlingR2 slingR { get; set; }
        public FlasherFire flasherFire { get; set; }
        public FlasherFear flasherFear { get; set; }
        public FlasherDeath flasherDeath { get; set; }
        public FlasherMortis flasherMortis { get; set; }
        public FlasherPursuitL flasherPursuitL { get; set; }
        public FlasherPursuitR flasherPursuitR { get; set; }
        public FlasherBlackout flasherBlackout { get; set; }
        public FlasherCursedEarth flasherCursedEarth { get; set; }
        public FlashersLowerLeft flashersLowerLeft { get; set; }
        public FlasherGlobe flasherGlobe { get; set; }
        public FlashersRtRamp flashersRtRamp { get; set; }
        public FlashersInsert flashersInsert { get; set; }
    }

    public class Perp1W
    {
        public string number { get; set; }
    }

    public class Perp1R
    {
        public string number { get; set; }
    }

    public class Perp1Y
    {
        public string number { get; set; }
    }

    public class Perp1G
    {
        public string number { get; set; }
    }

    public class Perp2W
    {
        public string number { get; set; }
    }

    public class Perp2R
    {
        public string number { get; set; }
    }

    public class Perp2Y
    {
        public string number { get; set; }
    }

    public class Perp2G
    {
        public string number { get; set; }
    }

    public class Perp4W
    {
        public string number { get; set; }
    }

    public class Perp4R
    {
        public string number { get; set; }
    }

    public class Perp4Y
    {
        public string number { get; set; }
    }

    public class Perp4G
    {
        public string number { get; set; }
    }

    public class Perp5W
    {
        public string number { get; set; }
    }

    public class Perp5R
    {
        public string number { get; set; }
    }

    public class Perp5Y
    {
        public string number { get; set; }
    }

    public class Perp5G
    {
        public string number { get; set; }
    }

    public class Perp3W
    {
        public string number { get; set; }
    }

    public class Perp3R
    {
        public string number { get; set; }
    }

    public class Perp3Y
    {
        public string number { get; set; }
    }

    public class Perp3G
    {
        public string number { get; set; }
    }

    public class Lock1
    {
        public string number { get; set; }
    }

    public class Lock2
    {
        public string number { get; set; }
    }

    public class Lock3
    {
        public string number { get; set; }
    }

    public class SuperGame2
    {
        public string number { get; set; }
    }

    public class CrimeLevel4
    {
        public string number { get; set; }
    }

    public class CrimeLevel3
    {
        public string number { get; set; }
    }

    public class CrimeLevel2
    {
        public string number { get; set; }
    }

    public class CrimeLevel1
    {
        public string number { get; set; }
    }

    public class Meltdown
    {
        public string number { get; set; }
    }

    public class Impersonator
    {
        public string number { get; set; }
    }

    public class BattleTank
    {
        public string number { get; set; }
    }

    public class StopMeltdown
    {
        public string number { get; set; }
    }

    public class Stakeout
    {
        public string number { get; set; }
    }

    public class Safecracker
    {
        public string number { get; set; }
    }

    public class Pursuit
    {
        public string number { get; set; }
    }

    public class UltChallenge
    {
        public string number { get; set; }
    }

    public class Manhunt
    {
        public string number { get; set; }
    }

    public class Blackout
    {
        public string number { get; set; }
    }

    public class Sniper
    {
        public string number { get; set; }
    }

    public class PickAPrize
    {
        public string number { get; set; }
    }

    public class ExtraBall2
    {
        public string number { get; set; }
    }

    public class RightStartFeature
    {
        public string number { get; set; }
    }

    public class TankCenter
    {
        public string number { get; set; }
    }

    public class AwardSniper
    {
        public string number { get; set; }
    }

    public class AirRade
    {
        public string number { get; set; }
    }

    public class LeftCenterFeature
    {
        public string number { get; set; }
    }

    public class TankLeft
    {
        public string number { get; set; }
    }

    public class Mystery2
    {
        public string number { get; set; }
    }

    public class DropTargetJ2
    {
        public string number { get; set; }
    }

    public class DropTargetU2
    {
        public string number { get; set; }
    }

    public class DropTargetD2
    {
        public string number { get; set; }
    }

    public class DropTargetG2
    {
        public string number { get; set; }
    }

    public class DropTargetE2
    {
        public string number { get; set; }
    }

    public class AwardSafecracker
    {
        public string number { get; set; }
    }

    public class MultiballJackpot
    {
        public string number { get; set; }
    }

    public class AwardBadImpersonator
    {
        public string number { get; set; }
    }

    public class AwardStakeout
    {
        public string number { get; set; }
    }

    public class BlackoutJackpot
    {
        public string number { get; set; }
    }

    public class DrainShield
    {
        public string number { get; set; }
        public string tags { get; set; }
    }

    public class JudgeAgain
    {
        public string number { get; set; }
    }

    public class AdvanceCrimeLevel
    {
        public string number { get; set; }
    }

    public class TankRight
    {
        public string number { get; set; }
    }

    public class BuyIn2
    {
        public string number { get; set; }
    }

    public class StartButton2
    {
        public string number { get; set; }
    }

    public class Gi01
    {
        public string number { get; set; }
    }

    public class Gi02
    {
        public string number { get; set; }
    }

    public class Gi03
    {
        public string number { get; set; }
    }

    public class Gi04
    {
        public string number { get; set; }
    }

    public class Gi05
    {
        public string number { get; set; }
    }

    public class PRLamps
    {
        public Perp1W perp1W { get; set; }
        public Perp1R perp1R { get; set; }
        public Perp1Y perp1Y { get; set; }
        public Perp1G perp1G { get; set; }
        public Perp2W perp2W { get; set; }
        public Perp2R perp2R { get; set; }
        public Perp2Y perp2Y { get; set; }
        public Perp2G perp2G { get; set; }
        public Perp4W perp4W { get; set; }
        public Perp4R perp4R { get; set; }
        public Perp4Y perp4Y { get; set; }
        public Perp4G perp4G { get; set; }
        public Perp5W perp5W { get; set; }
        public Perp5R perp5R { get; set; }
        public Perp5Y perp5Y { get; set; }
        public Perp5G perp5G { get; set; }
        public Perp3W perp3W { get; set; }
        public Perp3R perp3R { get; set; }
        public Perp3Y perp3Y { get; set; }
        public Perp3G perp3G { get; set; }
        public Lock1 lock1 { get; set; }
        public Lock2 lock2 { get; set; }
        public Lock3 lock3 { get; set; }
        public SuperGame2 superGame { get; set; }
        public CrimeLevel4 crimeLevel4 { get; set; }
        public CrimeLevel3 crimeLevel3 { get; set; }
        public CrimeLevel2 crimeLevel2 { get; set; }
        public CrimeLevel1 crimeLevel1 { get; set; }
        public Meltdown meltdown { get; set; }
        public Impersonator impersonator { get; set; }
        public BattleTank battleTank { get; set; }
        public StopMeltdown stopMeltdown { get; set; }
        public Stakeout stakeout { get; set; }
        public Safecracker safecracker { get; set; }
        public Pursuit pursuit { get; set; }
        public UltChallenge ultChallenge { get; set; }
        public Manhunt manhunt { get; set; }
        public Blackout blackout { get; set; }
        public Sniper sniper { get; set; }
        public PickAPrize pickAPrize { get; set; }
        public ExtraBall2 extraBall2 { get; set; }
        public RightStartFeature rightStartFeature { get; set; }
        public TankCenter tankCenter { get; set; }
        public AwardSniper awardSniper { get; set; }
        public AirRade airRade { get; set; }
        public LeftCenterFeature leftCenterFeature { get; set; }
        public TankLeft tankLeft { get; set; }
        public Mystery2 mystery { get; set; }
        public DropTargetJ2 dropTargetJ { get; set; }
        public DropTargetU2 dropTargetU { get; set; }
        public DropTargetD2 dropTargetD { get; set; }
        public DropTargetG2 dropTargetG { get; set; }
        public DropTargetE2 dropTargetE { get; set; }
        public AwardSafecracker awardSafecracker { get; set; }
        public MultiballJackpot multiballJackpot { get; set; }
        public AwardBadImpersonator awardBadImpersonator { get; set; }
        public AwardStakeout awardStakeout { get; set; }
        public BlackoutJackpot blackoutJackpot { get; set; }
        public DrainShield drainShield { get; set; }
        public JudgeAgain judgeAgain { get; set; }
        public AdvanceCrimeLevel advanceCrimeLevel { get; set; }
        public TankRight tankRight { get; set; }
        public BuyIn2 buyIn { get; set; }
        public StartButton2 startButton { get; set; }
        public Gi01 gi01 { get; set; }
        public Gi02 gi02 { get; set; }
        public Gi03 gi03 { get; set; }
        public Gi04 gi04 { get; set; }
        public Gi05 gi05 { get; set; }
    }

    public class ResetSwitches
    {
        public string shooterL { get; set; }
        public string outlaneL { get; set; }
        public string inlaneL { get; set; }
        public string threeBankTargets { get; set; }
        public string rightTopPost { get; set; }
        public string captiveBall1 { get; set; }
        public string mystery { get; set; }
        public string leftRampEnter { get; set; }
        public string leftRollover { get; set; }
        public string inlaneR { get; set; }
        public string topCenterRollover { get; set; }
        public string leftScorePost { get; set; }
        public string subwayEnter1 { get; set; }
        public string subwayEnter2 { get; set; }
        public string shooterR { get; set; }
        public string outlaneR { get; set; }
        public string inlaneFarR { get; set; }
        public string slingL { get; set; }
        public string slingR { get; set; }
        public string captiveBall2 { get; set; }
        public string dropTargetJ { get; set; }
        public string dropTargetU { get; set; }
        public string dropTargetD { get; set; }
        public string dropTargetG { get; set; }
        public string dropTargetE { get; set; }
        public string trough1 { get; set; }
        public string leftRampToLock { get; set; }
        public string leftRampExit { get; set; }
        public string rightRampEnter { get; set; }
        public string centerRampExit { get; set; }
        public string captiveBall3 { get; set; }
        public string topRightOpto { get; set; }
        public string topRampExit { get; set; }
        public string rightRampExit { get; set; }
        public string globePosition2 { get; set; }
    }

    public class StopSwitches
    {
        public string shooterR { get; set; }
        public string shooterL { get; set; }
        public string popperL { get; set; }
        public string popperR { get; set; }
        public string flipperLwL { get; set; }
        public string flipperLwR { get; set; }
    }
}
