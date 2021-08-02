using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public class GraphSaveUtility
{
    private DialogueTreeView targetTree;
    private DialogueContainer loadcontainer;
    private List<Edge> Edges => targetTree.edges.ToList();
    private List<DialogueNode> Nodes => targetTree.nodes.ToList().Cast<DialogueNode>().ToList();

 


    public static GraphSaveUtility GetInstance(DialogueTreeView graphView)
    {
        return new GraphSaveUtility
        {
            targetTree = graphView
        };
    }
    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return;
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();

        for (var i = 0; i < connectedSockets.Count(); i++)
        {
            var outputNode = (connectedSockets[i].output.node as DialogueNode);
            var inputNode = (connectedSockets[i].input.node as DialogueNode);
            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outputNode.GUID,
                PortName = connectedSockets[i].output.portName,
                TargetNodeGUID = inputNode.GUID
            });
        }

        foreach (var node in Nodes.Where(node => !node.EntryInp))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData{ NodeGUID = node.GUID, DialogueText = node.Dialogue, Position = node.GetPosition().position});
        }
        AssetDatabase.CreateAsset(dialogueContainer, path: $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }
    public void LoadNarrative(string fileName)
    {
        loadcontainer = Resources.Load<DialogueContainer>(fileName);
        if(loadcontainer == null)
        {
            EditorUtility.DisplayDialog(title: "File Not Found", message: "can't find file", ok: "OK" );
            return;
        }
        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }
    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryInp).GUID = loadcontainer.NodeLinks[0].BaseNodeGUID;
        foreach (var perNode in Nodes)
        {
            if (perNode.EntryInp) continue;
            Edges.Where(x => x.input.node == perNode).ToList().ForEach(edge => targetTree.RemoveElement(edge));
            targetTree.RemoveElement(perNode);
        }
    }
    private void CreateNodes()
    {
        foreach (var nodeData in loadcontainer.DialogueNodeData)
        {
            var tempnode = targetTree.CreateDialogueNode(nodeData.DialogueText);
            tempnode.GUID = nodeData.NodeGUID;
            targetTree.AddElement(tempnode);

            var nodeports  = loadcontainer.NodeLinks.Where(x => x.BaseNodeGUID == nodeData.NodeGUID).ToList();
            nodeports.ForEach(x => targetTree.AddChoicePort(tempnode, x.PortName));

        }
    }
    private void ConnectNodes()
    {
        for (var i = 0; i < Nodes.Count; i++)
        {
            var k = i;
            List<NodeLinkData> connections  = loadcontainer.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
            for (var j = 0; j < connections.Count(); j++)
            {
                var targetNodeGUID = connections[j].TargetNodeGUID;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                Port tempout = Nodes[i].outputContainer[j].Q<Port>(); ;
                Port tempin = (Port)targetNode.inputContainer[0];
                LinkNodesTogether(tempout, tempin);

                targetNode.SetPosition(new Rect(loadcontainer.DialogueNodeData.First(x => x.NodeGUID == targetNodeGUID).Position, targetTree.defaultSize));
            }
        }
    }
    private void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        targetTree.Add(tempEdge);
    }
}
