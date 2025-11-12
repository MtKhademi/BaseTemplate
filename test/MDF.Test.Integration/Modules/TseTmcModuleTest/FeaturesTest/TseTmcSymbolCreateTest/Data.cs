using MDF.Modules.Common;
using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TseTmcModule.Contract.Dtos;
using TseTmcModule.Contract.Enumerations;
using TseTmcWcfService;

namespace MDF.Test.Integration.Modules.TseTmcModuleTest.FeaturesTest.TseTmcSymbolCreateTest;

internal class Data
{
    private ArrayOfXElement GetArrayOfElements(string nameFileXml, string baseAddress = nameof(TseTmcSymbolUpdateTest))
    {
        var addressInstrumentTseTmc = Path.Combine(Directory.GetCurrentDirectory(),
          nameof(Modules),
          nameof(TseTmcModuleTest),
          nameof(FeaturesTest),
          baseAddress, nameFileXml);


        var node1 = @"
<xs:schema id=""Instruments"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""Instruments"" msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""TseInstruments"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""DEVen"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""InsCode"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""InstrumentID"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CValMne"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSocCSAC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LSoc30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18AFC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CIsin"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""QNmVlo"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""ZTitad"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""DESop"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YOPSJ"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""CGdSVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CGrValCot"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DInMar"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YUniExpP"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""YMarNSC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CComVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSoSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""YDeComp"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""PSaiSMaxOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""PSaiSMinOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""BaseVol"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""YVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""QPasCotFxeVal"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""QQtTranMarVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""Flow"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""QtitMinSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""QtitMaxSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""Valid"" type=""xs:unsignedByte"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>
";
        var node2 = new StreamReader(addressInstrumentTseTmc).ReadToEnd();

        var arrayOfElement = new ArrayOfXElement();
        arrayOfElement.Nodes.Add(XElement.Parse(node1));
        arrayOfElement.Nodes.Add(XElement.Parse(node2));
        return arrayOfElement;
    }
    private ArrayOfXElement GetArrayOfNoElements()
    {
        var addressInstrumentTseTmc = Path.Combine(Directory.GetCurrentDirectory(),
          nameof(Modules),
          nameof(TseTmcModuleTest),
          nameof(FeaturesTest),
          nameof(TseTmcSymbolUpdateTest),
          "TseTmcSymbolesNoElement.xml");


        var node1 = @"
<xs:schema id=""Instruments"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""Instruments"" msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""TseInstruments"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""DEVen"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""InsCode"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""InstrumentID"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CValMne"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSocCSAC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LSoc30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18AFC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CIsin"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""QNmVlo"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""ZTitad"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""DESop"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YOPSJ"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""CGdSVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CGrValCot"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DInMar"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YUniExpP"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""YMarNSC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CComVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSoSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""YDeComp"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""PSaiSMaxOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""PSaiSMinOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""BaseVol"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""YVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""QPasCotFxeVal"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""QQtTranMarVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""Flow"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""QtitMinSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""QtitMaxSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""Valid"" type=""xs:unsignedByte"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>
";
        var node2 = new StreamReader(addressInstrumentTseTmc).ReadToEnd();

        var arrayOfElement = new ArrayOfXElement();
        arrayOfElement.Nodes.Add(XElement.Parse(node1));
        arrayOfElement.Nodes.Add(XElement.Parse(node2));
        return arrayOfElement;
    }

    internal Mock<TsePublicV2Soap> TseTmcMock(string nameFileXml, string baseAddress = nameof(TseTmcSymbolUpdateTest))
    {
        var tseMoq = new Mock<TsePublicV2Soap>();
        tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)TseTmcFlow.Common))
          .ReturnsAsync(GetArrayOfElements(nameFileXml, baseAddress));

        for (int i = 1; i <= 7; i++)
        {
            tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)i))
              .ReturnsAsync(GetArrayOfNoElements());
        }


        return tseMoq;
    }


    internal async Task<IEnumerable<TseInstrumentDto>> GetInstruments(string nameFileXml, string baseAddress = nameof(TseTmcSymbolUpdateTest))
    {
        var elements = GetArrayOfElements(nameFileXml, baseAddress);
        var instrumentResultNode = elements.Nodes[1];

        XmlSerializer serializer = new(typeof(TseInstrumentDto));

        using var memoryStream = new MemoryStream();

        instrumentResultNode.Save(memoryStream);
        memoryStream.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        var xmlReaderSettings = new XmlReaderSettings { IgnoreWhitespace = true };
        var xmlReader = XmlReader.Create(memoryStream, xmlReaderSettings);
        return Normalize(XDocument.Load(xmlReader)
            .Descendants("TseInstruments")
            .Select(i => serializer.Deserialize(i.CreateReader()))
            .Cast<TseInstrumentDto>()
            .ToArray());
    }
    private static IEnumerable<TseInstrumentDto> Normalize(IEnumerable<TseInstrumentDto> symbols)
    {
        foreach (var symbol in symbols)
        {
            symbol.PersianCompany30DigitName = symbol.PersianCompany30DigitName!.FixPersianChars();
            symbol.PersianSymbol130DigitName = symbol.PersianSymbol130DigitName!.FixPersianChars();
            symbol.PersianSymbol18DigitCode = symbol.PersianSymbol18DigitCode!.FixPersianChars();
        }
        return symbols;
    }
}


public class TseTmcMockForCheckUpdate
{

    public Mock<TsePublicV2Soap> TseTmcMoq { get; set; }


    private ArrayOfXElement GetArrayOfElements(string nameFileXml, string baseAddress = nameof(TseTmcSymbolUpdateTest))
    {
        var addressInstrumentTseTmc = Path.Combine(Directory.GetCurrentDirectory(),
          nameof(Modules),
          nameof(TseTmcModuleTest),
          nameof(FeaturesTest),
          baseAddress, nameFileXml);

        //D:\source\mdf\test\MDF.Test\bin\Debug\net8.0\ModulesTest\TseTmcModuleTest\FeaturesTest\TseTmcSymbolUpdateTest\TseTmcSymboles7.xml
        //D:\\source\\mdf\\test\\MDF.Test\\bin\\Debug\\net8.0\\ModulesTest\\TseTmcModuleTest\\FeaturesTest\\TseTmcSymbolUpdateTest
        //"D:\\source\\mdf\\test\\MDF.Test\\bin\\Debug\\net8.0\\ModulesTest\\TseTmcModuleTest\\FeaturesTest\\TseTmcSymbolUpdateTest\\TseTmcSymboles7.xml"

        var xx = File.Exists(addressInstrumentTseTmc);

        var node1 = @"
<xs:schema id=""Instruments"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""Instruments"" msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""TseInstruments"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""DEVen"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""InsCode"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""InstrumentID"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CValMne"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSocCSAC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LSoc30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18AFC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CIsin"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""QNmVlo"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""ZTitad"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""DESop"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YOPSJ"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""CGdSVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CGrValCot"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DInMar"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YUniExpP"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""YMarNSC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CComVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSoSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""YDeComp"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""PSaiSMaxOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""PSaiSMinOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""BaseVol"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""YVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""QPasCotFxeVal"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""QQtTranMarVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""Flow"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""QtitMinSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""QtitMaxSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""Valid"" type=""xs:unsignedByte"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>
";
        var node2 = new StreamReader(addressInstrumentTseTmc).ReadToEnd();

        var arrayOfElement = new ArrayOfXElement();
        arrayOfElement.Nodes.Add(XElement.Parse(node1));
        arrayOfElement.Nodes.Add(XElement.Parse(node2));
        return arrayOfElement;
    }
    private ArrayOfXElement GetArrayOfNoElements()
    {
        var addressInstrumentTseTmc = Path.Combine(Directory.GetCurrentDirectory(),
          nameof(Modules),
          nameof(TseTmcModuleTest),
          nameof(FeaturesTest),
          nameof(TseTmcSymbolUpdateTest),
          "TseTmcSymbolesNoElement.xml");


        var node1 = @"
<xs:schema id=""Instruments"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""Instruments"" msdata:IsDataSet=""true"" msdata:UseCurrentLocale=""true"">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""TseInstruments"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""DEVen"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""InsCode"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""InstrumentID"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CValMne"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSocCSAC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LSoc30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal18AFC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""LVal30"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CIsin"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""QNmVlo"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""ZTitad"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""DESop"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YOPSJ"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""CGdSVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CGrValCot"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DInMar"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""YUniExpP"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""YMarNSC"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CComVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""CSoSecVal"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""YDeComp"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""PSaiSMaxOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""PSaiSMinOkValMdv"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""BaseVol"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""YVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""QPasCotFxeVal"" type=""xs:decimal"" minOccurs=""0"" />
              <xs:element name=""QQtTranMarVal"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""Flow"" type=""xs:unsignedByte"" minOccurs=""0"" />
              <xs:element name=""QtitMinSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""QtitMaxSaiOmProd"" type=""xs:long"" minOccurs=""0"" />
              <xs:element name=""Valid"" type=""xs:unsignedByte"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>
";
        var node2 = new StreamReader(addressInstrumentTseTmc).ReadToEnd();

        var arrayOfElement = new ArrayOfXElement();
        arrayOfElement.Nodes.Add(XElement.Parse(node1));
        arrayOfElement.Nodes.Add(XElement.Parse(node2));
        return arrayOfElement;
    }
    public Mock<TsePublicV2Soap> GetTseTmcMo(string nameFileXml = "TseTmcSymboles5.xml", string baseAddress = nameof(TseTmcSymbolUpdateTest))
    {
        var tseMoq = new Mock<TsePublicV2Soap>();
        tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)TseTmcFlow.Common))
          .ReturnsAsync(GetArrayOfElements(nameFileXml, baseAddress));

        for (int i = 1; i <= 7; i++)
        {
            tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)i))
              .ReturnsAsync(GetArrayOfNoElements());
        }


        return tseMoq;
    }


    internal async Task<IEnumerable<TseInstrumentDto>> GetInstruments(string nameFileXml, string baseAddress = nameof(TseTmcSymbolUpdateTest))
    {
        var elements = GetArrayOfElements(nameFileXml, baseAddress);
        var instrumentResultNode = elements.Nodes[1];

        XmlSerializer serializer = new(typeof(TseInstrumentDto));

        using var memoryStream = new MemoryStream();

        instrumentResultNode.Save(memoryStream);
        memoryStream.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        var xmlReaderSettings = new XmlReaderSettings { IgnoreWhitespace = true };
        var xmlReader = XmlReader.Create(memoryStream, xmlReaderSettings);
        return Normalize(XDocument.Load(xmlReader)
            .Descendants("TseInstruments")
            .Select(i => serializer.Deserialize(i.CreateReader()))
            .Cast<TseInstrumentDto>()
            .ToArray());
    }
    private static IEnumerable<TseInstrumentDto> Normalize(IEnumerable<TseInstrumentDto> symbols)
    {
        foreach (var symbol in symbols)
        {
            symbol.PersianCompany30DigitName = symbol.PersianCompany30DigitName!.FixPersianChars();
            symbol.PersianSymbol130DigitName = symbol.PersianSymbol130DigitName!.FixPersianChars();
            symbol.PersianSymbol18DigitCode = symbol.PersianSymbol18DigitCode!.FixPersianChars();
        }
        return symbols;
    }
}



public class DataForCreateSymbolFromTseTmcValid : TheoryData<
    string, string, SymbolResponseTest>
{
    public DataForCreateSymbolFromTseTmcValid()
    {
        Add("DataFromTseTmc.xml", "IROAMOJF2611", new SymbolResponseTest
        {
            Isin = "IROAMOJF2611",
            SymbolName = "اختیارف سینرژی-20000-14031101",
            TypeOfSymbol = TypeOfSymbolTest.Stock,
            TypeOfSymbolInTseTmc = TypeOfSymbolTest.Forward_Day,
        });
    }
}
