using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Enumerations;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TypeOfSymbolTest : byte
{

    [Description("نامشخص")]
    Undefined = 0,

    /// <summary>
    /// سهام عادی 
    /// </summary>
    [Description("سهام عادی")]
    Stock,

    /// <summary>
    /// اوراق 
    /// صکوک اختصاصی
    /// اوراق مشارکت 
    /// اوراق مشارکت صکوک
    /// اوراق مشارکت (مظنه یابی) - اوراق گام
    /// اوراق امتیاز ارز صادراتی
    /// اوراق مشارکت کالا
    /// </summary>
    [Description("اوراق مشارکت")]
    Bond,

    [Description("اوراق اجاره")]
    Bond_Ejare,

    [Description("اوراق مرابحه")]
    Bond_Morabehe,

    [Description("اوراق گواهی اعتبار مولد")]
    Bond_GovahiEhtebarMovaled,


    [Description("اوراق خزانه اسلامی")]
    Bond_KhazanehEslami,

    [Description("اوراق سلف")]
    Bond_Salaf,

    [Description("اوراق صکوک مرابحه")]
    Bond_Sakok_Morabehe,
    [Description("اوراق صکوک اجاره")]
    Bond_Sakok_Ejare,


    [Description("اوراق قرض الحسنه")]
    Bond_GharzolHasane,

    [Description("اوراق قرضه/دین")]
    Bond_Debentures,


    /// <summary>
    /// کالا
    /// گواهی سپرده کالایی ناپیوسته
    /// گواهی سپرده کالایی
    /// </summary>
    [Description("گواهی سپرده کالایی")]
    Commodity,

    /// <summary>
    /// شاخص
    /// شاخص فرابورس
    /// شاخص قیمت
    /// </summary>
    [Description("شاخص")]
    Index,

    /// <summary>
    /// صندوق صدور ابطالی کالایی
    /// مثل کالایی در بورس کالا - زعفران یا شمش در بورس کالا
    /// </summary>
    [Description("صندوق صدور ابطالی کالایی")]
    NTF_Commodity,

    /// <summary>
    /// صندوق صدور ابطالی که فقط سهام دارد
    /// مثل پیشتاز
    /// </summary>
    [Description("صندوق صدور ابطالی سهامی")]
    NTF_Stock,

    /// <summary>
    /// صندوق صدور ابطالی درامد ثابت
    /// مثل نامی - حامی
    /// </summary>
    [Description("صندوق صدور ابطالی درامد ثابت")]
    NTF_Bond,

    /// <summary>
    /// صندوق صدور ابطالی مختلط
    /// هم سهام و هم اوراق درامد ثابت بخرد
    /// </summary>
    [Description("صندوق صدور ابطالی مختلط")]
    NTF_Mixed,

    /// <summary>
    /// صندوق سرمایه گذاری قابل معامله انرژی
    /// </summary>
    [Description("صندوق صدور ابطالی انرژی")]
    ETF_Energy,

    /// <summary>
    /// صندوق قابل معامله از سهام های بورس کالا
    /// مثل عیار
    /// </summary>
    [Description("صندوق قابل معامله بورس کالایی")]
    ETF_Commodity,


    /// <summary>
    /// صندوق سهامی
    /// </summary>
    [Description("صندوق قابل معامله سهامی")]
    ETF_Stock,

    /// <summary>
    /// صندوق درامد ثابت قابل معامله
    /// </summary>
    [Description("صندوق قابل معامله درامد ثابت")]
    ETF_Bond,

    /// <summary>
    /// صندوق سهام و اوراق مختلط قابل معامله
    /// </summary>
    [Description("صندوق قابل معامله مختلط")]
    ETF_Mixed,

    /// <summary>
    /// صندوق زمین و ساختمان
    /// </summary>
    [Description("صندوق قابل زمین و ساختمان")]
    ETF_RealEstate,


    /// <summary>
    /// صندوق های قابل معامله ای
    /// صندوق های جسورانه ای هستن
    /// مثلا یک شرکت میگه من میخوام یه ماده جدید بسازم اینها میرن روش سرمایه گذاری میکنن
    /// روی شرکت های دانش بنیان سرمایه گذاری میکنن
    /// </summary>
    [Description("صندوق جسورانه")]
    ETF_Venture,

    /// <summary>
    /// صندوق های قابل معامله خصوصی
    /// سهام شرکت هایی که در بورس عرضه نشدن رو میخرن
    /// سهام شرکت هایی سهامی خاص
    /// </summary>
    [Description("صندوق قابل معامله خصوصی")]
    ETF_Private,

    /// <summary>
    /// صندوق قابل معامله که فقط صندوق های 
    /// دیگر رو خرید و فروش میکنه
    /// </summary>
    [Description("صندوق قابل معامله دارای صندوق")]
    ETF_FundOfFund,

    /// <summary>
    /// صندوق قابل معامله که هر بخش آن باید در یک صنعت خاص سرمایه گذاری کند
    /// </summary>
    [Description("صندوق قابل معامله بخشی")]
    ETF_Sector,

    /// <summary>
    /// سلف بورس انرژی روز
    /// </summary>
    [Description("سلف (بورس انرژی_روز)")]
    Forward_Day,
    /// <summary>
    /// سلف بورس انرژی هفته
    /// </summary>
    [Description("سلف (بورس انرژی_هفته)")]
    Forward_Week,
    /// <summary>
    /// سلف بورس انرژی ماه
    /// </summary>
    [Description("سلف (بورس انرژی_ماه)")]
    Forward_Month,
    /// <summary>
    /// سلف بورس انرژی فصل
    /// </summary>
    [Description("سلف (بورس انرژی_فصل)")]
    Forward_Quarter,
    /// <summary>
    /// سلف بورس انرژی سال
    /// </summary>
    [Description("سلف (بورس انرژی_سال)")]
    Forward_Year,

    /// <summary>
    /// اختیار ف اخزا
    /// اختیار فروش اخزا
    ///  اختیار فروش اسناد خزانه داری اسلامی
    /// </summary>
    [Description("اختیار فروش اخزا")]
    BondPutOption,// Related

    /// <summary>
    /// اختیار خ اخزا
    /// اختیار خرید اخزا
    /// اختیار خرید اسناد خزانه داری اسلامی
    /// </summary>
    [Description("اختیار خرید اخزا")]
    BondCallOption,// Related
    /// <summary>
    /// حق تقدم سهم
    /// حق تقدم شرکت های کوچک و متوسط
    /// </summary>
    [Description("سهم حق تقدم")]
    PreemptiveRight,// Related

    /// <summary>
    /// اختیار فروش سهام بورس
    /// اختیار فروش سهام فرابورس
    /// </summary>
    [Description("اختیار فروش سهام")]
    PutOption, // Related

    /// <summary>
    /// اختیار خرید
    /// اختیار خرید سهام فرابورس
    /// </summary>
    [Description("اختیار خرید سهام")]
    CallOption,// Related

    /// <summary>
    /// اختیار خرید تبعی فرابورس
    /// اختیار خرید تبعی
    /// </summary>
    [Description("اختیار خرید تبعی")]
    CallEmbedded,// Related

    /// <summary>
    /// اختیار فروش تبعی
    /// اختیار فروش تبعی فرابورس
    /// </summary>
    [Description("اختیار فروش تبعی")]
    PutEmbedded,// Related,

    /// <summary>
    /// آتی
    /// </summary>
    [Description("آتی")]
    Future,// Related


}
