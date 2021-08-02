using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
public class DialogueTreeView : GraphView 
{
    public readonly Vector2 defaultSize = new Vector2(x: 150, y: 200);

    public DialogueTreeView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

       AddElement(GenerateFirstNode());
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        var startPortView = startPort;

        ports.ForEach((port) =>
        {
            var portView = port;
            if (startPortView != portView && startPortView.node != portView.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private DialogueNode GenerateFirstNode()
    {
        var node = new DialogueNode
        {
            title = "Initial",
            GUID = Guid.NewGuid().ToString(),
            Dialogue = "FillerStart",
            EntryInp = true
        };
        var genPort = GeneratePort(node, Direction.Output);
        genPort.portName = "Next";
        node.outputContainer.Add(genPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }
    public void CreateNode(string nname)
    {
        AddElement(CreateDialogueNode(nname));
    }
    public DialogueNode CreateDialogueNode(string nodenm)
    {
        var NewNode = new DialogueNode
        {
            title = nodenm,
            GUID = Guid.NewGuid().ToString(),
            Dialogue = nodenm,
        };
        var inport = GeneratePort(NewNode, Direction.Input, Port.Capacity.Multi);
        inport.portName = "Input";
        NewNode.inputContainer.Add(inport);

        var button = new Button(clickEvent: () => { AddChoicePort(NewNode); });
        button.text = "New Choice";

        NewNode.titleContainer.Add(button);

        var textField = new TextField("");
        textField.RegisterValueChangedCallback(evt =>
        {
            NewNode.Dialogue = evt.newValue;
            NewNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(NewNode.title);
        NewNode.mainContainer.Add(textField);

        NewNode.RefreshExpandedState();
        NewNode.RefreshPorts();
        NewNode.SetPosition(new Rect(position: Vector2.zero, defaultSize));

        return NewNode;
    }

    public void AddChoicePort(DialogueNode thenode, string overriddenPortName = "")
    {
        var genport = GeneratePort(thenode, Direction.Output);
        var portLabel = genport.contentContainer.Q<Label>(name:"type");
        genport.contentContainer.Remove(portLabel);
        
        var outputPortCount = thenode.outputContainer.Query(name: "connector").ToList().Count;
        var outputPortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Option {outputPortCount + 1}"
            : overriddenPortName;


        var textField = new TextField()
        {
            name = string.Empty,
            value = outputPortName
        };
        textField.RegisterValueChangedCallback(evt => genport.portName = evt.newValue);
        genport.contentContainer.Add(new Label("  "));
        genport.contentContainer.Add(textField);
        var deleteButton = new Button(() => RemovePort(thenode, genport))
        {
            text = "X"
        };
        genport.contentContainer.Add(deleteButton);
        genport.portName = outputPortName;
        thenode.inputContainer.Add(genport);
        thenode.RefreshPorts();
        thenode.RefreshExpandedState();

    }
    private void RemovePort(Node node, Port socket)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
        if (!targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.output.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        node.outputContainer.Remove(socket);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }
}
