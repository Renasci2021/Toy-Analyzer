using System.Xml.Linq;

namespace ToyAnalyzer.Parser.Common;

public interface IParser
{
    XElement Parse();
}
