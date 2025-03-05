using Godot;
using System;

public partial class ApiController : Node
{
    
    HttpRequest requester;

    public override void _Ready(){
        requester = GetNode<HttpRequest>("HttpRequest");
    }
    
    public void _on_resume_pressed(){
        requester.Request("https://nthapiweb.onrender.com");
    }

}
