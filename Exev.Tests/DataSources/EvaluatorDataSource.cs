using System.Collections;
using System.Collections.Generic;

namespace Exev.Tests.DataSources;
public class EvaluatorDataSource : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "1 + 2", 3 };
        yield return new object[] { "- 1", -1 };
        yield return new object[] { "--1", 1 };
        yield return new object[] { "1 + -1", 0 };
        yield return new object[] { "2 ^ 2 ^ 3", 256 };
        yield return new object[] { "- 2 ^ 2 ^ 3", -256 };
        yield return new object[] { "20 * 2 / 5", 8 };
        yield return new object[] { "(-(-20.8))", 20.8 };
        yield return new object[] { "-((-(-20)) * (1 + 1))", -40 };
        yield return new object[] { "(5-(6/2))+(3*4)", 14 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
