using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;



public partial class GDpsx_MessageBox : Label
{
    private int timePerCharMS = 25;
    private int charIndex = 0;
    private string message = "";
    private AudioStreamPlayer2D audioPlayer;
    private bool InUse = false;

    public override void _Ready()
    {
        audioPlayer = GetNode<AudioStreamPlayer2D>("TypeSFX");
    }

    private void PlayTypeSFX()
    {
        audioPlayer.PitchScale = (float)GD.RandRange(0.7f, 1.3f);
        audioPlayer.Play();
    }
    


    public void StartTyping(string _message, int _timePerCharMS = 25, int _messageHoldTime = 2000)
    {
        if(InUse) return;
        audioPlayer.Stream = (AudioStream)ResourceLoader.Load("res://addons/GDpsx/Audio/UI/messagebox_typewriter.wav");
        timePerCharMS = _timePerCharMS;
        message = _message;
        FadeIn();
        performTypingEffect(_messageHoldTime);
    }

    private async void performTypingEffect(int _messageHoldTime)
    {
        InUse = true;
        for (int i = 0; i < message.Length + 1; i++)
        {
            PlayTypeSFX();
            charIndex = i;
            VisibleCharacters = charIndex;
            Text = message.Substring(0, i);
            await Task.Delay(timePerCharMS);
        }
        await Task.Delay(_messageHoldTime);
        FadeOut();
    }

    private async void FadeOut()
    {
        for (float i = 1; i >= 0; i -= 0.1f)
        {
            SelfModulate = new Color(1, 1, 1, i);
            await Task.Delay(100);
        }
        SelfModulate = new Color(1, 1, 1, 0);
        message = "";
        
        InUse = false;
    }

    private async void FadeIn()
    {
        for (float i = 0; i <= 1; i += 0.1f)
        {
            SelfModulate = new Color(1, 1, 1, i);
            await Task.Delay(100);
        }

    }
}