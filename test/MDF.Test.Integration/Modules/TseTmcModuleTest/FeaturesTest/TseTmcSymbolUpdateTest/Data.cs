using MDF.Modules.Common;
using Moq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TseTmcModule.Contract.Dtos;
using TseTmcModule.Contract.Enumerations;
using TseTmcWcfService;

namespace MDF.Test.Integration.Modules.TseTmcModuleTest.FeaturesTest.TseTmcSymbolUpdateTest;

internal class Data
{
    private ArrayOfXElement GetArrayOfElements(string nameFileXml)
    {
        var addressInstrumentTseTmc = Path.Combine(Directory.GetCurrentDirectory(),
          nameof(Modules),
          nameof(TseTmcModuleTest),
          nameof(FeaturesTest),
          nameof(TseTmcSymbolUpdateTest), nameFileXml);


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

    internal Mock<TsePublicV2Soap> TseTmcMock(string nameFileXml)
    {
        var tseMoq = new Mock<TsePublicV2Soap>();
        tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)TseTmcFlow.Common))
          .ReturnsAsync(GetArrayOfElements(nameFileXml));

        for (int i = 1; i <= 7; i++)
        {
            tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)i))
              .ReturnsAsync(GetArrayOfNoElements());
        }


        return tseMoq;
    }


    internal async Task<IEnumerable<TseInstrumentDto>> GetInstruments(string nameFileXml)
    {
        var elements = GetArrayOfElements(nameFileXml);
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


    private ArrayOfXElement GetArrayOfElements(string nameFileXml)
    {
        var addressInstrumentTseTmc = Path.Combine(Directory.GetCurrentDirectory(),
          nameof(Modules),
          nameof(TseTmcModuleTest),
          nameof(FeaturesTest),
          nameof(TseTmcSymbolUpdateTest), nameFileXml);

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
    public Mock<TsePublicV2Soap> GetTseTmcMo(string nameFileXml = "TseTmcSymboles5.xml")
    {
        var tseMoq = new Mock<TsePublicV2Soap>();
        tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)TseTmcFlow.Common))
          .ReturnsAsync(GetArrayOfElements(nameFileXml));

        for (int i = 1; i <= 7; i++)
        {
            tseMoq.Setup(mt => mt.InstrumentAsync(It.IsAny<string>(), It.IsAny<string>(), (byte)i))
              .ReturnsAsync(GetArrayOfNoElements());
        }


        return tseMoq;
    }


    internal async Task<IEnumerable<TseInstrumentDto>> GetInstruments(string nameFileXml)
    {
        var elements = GetArrayOfElements(nameFileXml);
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



public class IsinsForCheckChangeTypeOfSymbolToBonds : TheoryData<List<(string isin, TypeOfSymbolTest typeOfSymbol,
    int? firmId, byte? boardCode, string? marketCode, byte? securitiesExchangeCode)>>
{
    public IsinsForCheckChangeTypeOfSymbolToBonds()
    {
        Add([

        ("IRB5AE8705C1", TypeOfSymbolTest.Bond, 5,9,"NO",9),
        ("IRB5AE2504C1", TypeOfSymbolTest.Bond, 6,4,"OL",11),
        ("IRB3TA010571", TypeOfSymbolTest.Bond_Ejare, 6,4,"OL",11),
        ("IRB3MA060571", TypeOfSymbolTest.Bond_Ejare,6,4,"OL",11),
        ("IRB7AE040231", TypeOfSymbolTest.Bond_GovahiEhtebarMovaled, 6,4,"OL",11),
        ("IRB3W0250261", TypeOfSymbolTest.Bond_GovahiEhtebarMovaled, 6,4,"OL",11),
        ("IRB3AR040761", TypeOfSymbolTest.Bond_Morabehe, 6,4,"OL",11),
        ("IRB3MD010661", TypeOfSymbolTest.Bond_Morabehe, 6,4,"OL",11),
        ("IRB4O03706C1", TypeOfSymbolTest.Bond_Morabehe, 6,4,"OL",11),
        ("IRB6AE9806A1", TypeOfSymbolTest.Bond_Morabehe, 6,4,"OL",11),
        ("IRB3TR060481", TypeOfSymbolTest.Bond_KhazanehEslami, 6,4,"NO",3),
        ("IRB3MI0103C1", TypeOfSymbolTest.Bond_GharzolHasane, 6,4,"OL",11),
        ("IRB5AE800041", TypeOfSymbolTest.CallOption, 7,4,"OL",11),

        ("IRBEC5010311", TypeOfSymbolTest.Bond_Salaf, 6,4,"OL",11),
        ("IRBEMT160411", TypeOfSymbolTest.Bond_Salaf, 6,4,"OL",11),
        ("IRB5AE800070", TypeOfSymbolTest.Bond_Salaf, 7,4,"OL",11),

        ("IRB6AF350761", TypeOfSymbolTest.Bond_Sakok_Morabehe, 7,4,"OL",11),
        ("IRB6AF330751", TypeOfSymbolTest.Bond_Sakok_Morabehe, 7,4,"OL",11),
        ("IRB6AF360661", TypeOfSymbolTest.Bond_Sakok_Morabehe, 7,4,"OL",11),
        ("IRB5AE800067", TypeOfSymbolTest.Bond_Sakok_Morabehe, 7,4,"OL",11),
        ("IRB6AF290541", TypeOfSymbolTest.Bond_Sakok_Morabehe, 7,4,"OL",11),

        ("IRB6AF370761", TypeOfSymbolTest.Bond_Sakok_Ejare, 7,4,"OL",11),
        ("IRB6AF340761", TypeOfSymbolTest.Bond_Sakok_Ejare, 7,4,"OL",11),
        ("IRB5AE800056", TypeOfSymbolTest.Bond_Sakok_Ejare, 7,4,"OL",11),

        ("IRB5AE800071", TypeOfSymbolTest.CallOption, 7,8,"NO",9),
        ("IRB3PA01037A1",TypeOfSymbolTest.Bond_Debentures, 7,8,"NO",9),

        ("IRB6SEFA0371", TypeOfSymbolTest.Bond_Sakok_Ejare, 7,8,"NO",9),
        ("IRB6NF0303C1", TypeOfSymbolTest.Bond_Sakok_Morabehe, 7,8,"NO",9),
        ("IRB6GO030391", TypeOfSymbolTest.Bond_Sakok_Ejare, 7,8,"NO",9),

        ("IRS4KTEK0011", TypeOfSymbolTest.PutEmbedded, 7,8,"NO",9),


        ]);
    }
}

public class SymbolUpdatedFromTseTmcData : TheoryData<List<
    (string isin, TypeOfSymbolTest typeOfSymbol, string symbolName, string symbolTitle, string symbolTseName)>>
{
    public SymbolUpdatedFromTseTmcData()
    {
        Add([

            ("KORI5558585", TypeOfSymbolTest.PutOption,"اختیارخ سینرژی-14000-14031101","اختیارخ سینرژی-14000-14031101","اختیارخ سینرژی-14000-14031101"),
            ("KORI5558586", TypeOfSymbolTest.PutOption,"اختیارف سینرژی-20000-14031101","اختیارف سینرژی-20000-14031101","اختیارف سینرژی-20000-14031101"),
            ("KORI5558587", TypeOfSymbolTest.Stock,"اختیارف سینرژی-20000-14031101","اختیارف سینرژی-20000-14031101","اختیارف سینرژی-20000-14031101"),
            ("IROTABNY0001", TypeOfSymbolTest.Stock,"ابنيه1","ابنيه1","ابنیه وساختمان پدیده شاندیز"),
            ("IROTTAKN0001", TypeOfSymbolTest.Stock,"تك نيرو1","تك نيرو1","تعاونی کارکنان نیروگاه یزد"),
        ]);
    }
}