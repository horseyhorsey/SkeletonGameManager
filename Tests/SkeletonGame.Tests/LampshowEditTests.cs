using SkeletonGame.Engine;
using Xunit;

namespace SkeletonGame.Tests
{
    public class LampshowEditTests
    {
        private ILampshowEdit _lampShowEdit;
        const string LAMP_FILE = @"TestData\EmptyGame\assets\lampshows\GoingUp.lampshow";
        const string LAMP_FILE_REVERSED = @"TestData\EmptyGame\assets\lampshows\GoingUpReversed.lampshow";

        [Fact]
        public void ReverseLampshowTest()
        {
            _lampShowEdit = new LampshowEdit();

            _lampShowEdit.ReverseLampshowFile(LAMP_FILE, LAMP_FILE_REVERSED);
        }
    }
}
