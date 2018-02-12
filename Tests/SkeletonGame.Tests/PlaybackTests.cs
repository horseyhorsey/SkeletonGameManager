﻿using SkeletonGame.Engine;
using SkeletonGame.Models;
using System;
using Xunit;

namespace SkeletonGame.Tests
{


    public class PlaybackTests
    {
        [Fact]
        public void ParsePlaybackFile___ItemsShouldBe80()
        {
            var lines = RecordingManager.ParsePlaybackFile(Environment.CurrentDirectory + @"\TestData\EmptyGame\recordings\playback.txt");

            Assert.True(lines.Count == 80);
        }


        string playback = @"
1|0
3|0
5|0
7|0
12|0
13|0
14|0
15|0
144|0
145|0
146|0
147|0
148|0
149|0
150|0
34|0
35|0
36|0
37|0
38|0
39|0
48|0
49|0
64|0
65|0
66|0
67|0
68|0
69|0
80|0
96|0
97|0
7498.0|1|34|startButton
7594.0|2|34|startButton
8218.0|1|34|startButton
8328.0|2|34|startButton
9661.0|1|80|shooter_lane
10639.0|2|80|shooter_lane
13008.0|1|65|mOnkey
13080.0|2|65|mOnkey
13448.0|1|64|Monkey
13568.0|2|64|Monkey
14272.0|1|66|moNkey
14360.0|2|66|moNkey
14552.0|1|67|monKey
14664.0|2|67|monKey
14856.0|1|68|monkEy
14960.0|2|68|monkEy
15208.0|1|69|monkeY
15288.0|2|69|monkeY
16725.0|2|144|trough1
17461.0|2|145|trough2
17854.0|2|146|trough3
18303.0|2|147|trough4
20299.0|1|80|shooter_lane
21701.0|2|80|shooter_lane
22417.0|1|147|trough4
24739.0|2|147|trough4
31785.0|1|80|shooter_lane
33601.0|2|80|shooter_lane
34604.0|1|147|trough4
35807.0|2|147|trough4
40502.0|1|147|trough4
44963.0|1|80|shooter_lane
48025.0|2|80|shooter_lane
49370.0|1|64|Monkey
49482.0|2|64|Monkey
49890.0|1|65|mOnkey
49989.0|2|65|mOnkey
50309.0|1|66|moNkey
50410.0|2|66|moNkey
50778.0|1|67|monKey
50874.0|2|67|monKey
51131.0|1|68|monkEy
51211.0|2|68|monkEy
51475.0|1|69|monkeY
51555.0|2|69|monkeY
54070.0|1|144|trough1
54603.0|2|144|trough1
56770.0|2|147|trough4
";
    }
}
