namespace ITCompanionAI.Ingestion.Docs;


public class DocIngester
{

    public async Task RunIngestion()
    {
        var connectionString = """server=(LocalDB)\MSSQLLocalDB;Database=KnowledgeCurator;Trusted_Connection=True;""";
        LearnPageParser parser = new();
        DocRepository repo = new(connectionString);
        LearnIngestionRunner runner = new(parser, repo);

        Guid sourceSnapshotId = Guid.Parse("9859fcf3-4085-45aa-a563-ddec9dc8329c");
        Guid ingestionRunId = Guid.Parse("997BE774-96B5-44D8-B6AD-14FC2EBDDABB");

        //Head of the Microsoft Learn Agent Framework docs
        await runner.IngestAsync("https://learn.microsoft.com/en-us/agent-framework/overview/agent-framework-overview");
    }
}





/*
 *
 *
 *
        var urls = new[]
       {
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.aggregatorprompttemplatefactory?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.aifunctionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.apimanifestkernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.audiocontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.autofunctionchoicebehavior?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.autofunctioninvocationcontext?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azureaiinferencekernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azureaiinferenceservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azureaisearchkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azureaisearchservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azurecosmosdbmongodbkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azurecosmosdbmongodbservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azurecosmosdbnosqlkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azurecosmosdbnosqlservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azureopenaikernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.azureopenaiservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.binarycontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.binarycontentextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.cancelkerneleventargs?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.chatmessagecontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.copilotagentpluginkernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.declarativeagentextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.echoprompttemplatefactory?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.filereferencecontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.fromkernelservicesattribute?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functioncallcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functioncallcontentbuilder?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functionchoicebehavior?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functionchoicebehaviorconfiguration?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functionchoicebehaviorconfigurationcontext?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functionchoicebehavioroptions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functioninvocationcontext?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functioninvokedeventargs?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functioninvokingeventargs?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functionresult?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.functionresultcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.googleaikernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.googleaimemorybuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.googleaiservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.handlebarskernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.httpoperationexception?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.huggingfacekernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.huggingfaceservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.imagecontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.inputvariable?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernel?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelarguments?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kerneleventargs?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelexception?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunction?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionattribute?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctioncanceledexception?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionfactory?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionfrommethodoptions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionmarkdown?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionmetadata?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionmetadatafactory?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelfunctionyaml?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kerneljsonschema?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelparametermetadata?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelplugin?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelplugincollection?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelpluginextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelpluginfactory?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelprompttemplatefactory?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.kernelreturnparametermetadata?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.markdownkernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.mistralaikernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.mistralaiservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.mongodbservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.nonefunctionchoicebehavior?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.ollamakernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.ollamaservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.onnxkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.onnxservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.openaichathistoryextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.openaikernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.openaiservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.openapikernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.outputvariable?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.pineconekernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.pineconeservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.postgresservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.promptexecutionsettings?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.promptrendercontext?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.promptrenderedeventargs?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.promptrenderingeventargs?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.prompttemplateconfig?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.prompttemplatefactoryextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.promptyamlkernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.promptykernelextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.qdrantkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.qdrantservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.rediskernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.redisservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.requiredfunctionchoicebehavior?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.restapioperationresponse?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.restapioperationresponseconverter?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.restapioperationresponseextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.sqliteservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.streamingchatmessagecontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.streamingfilereferencecontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.streamingfunctioncallupdatecontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.streamingkernelcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.streamingmethodcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.streamingtextcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.textcontent?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.textsearchkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.textsearchservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.vertexaikernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.vertexaimemorybuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.vertexaiservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.weaviatekernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.weaviateservicecollectionextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.webkernelbuilderextensions?view=semantic-kernel-dotnet",
    "https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.webservicecollectionextensions?view=semantic-kernel-dotnet"

};
*/