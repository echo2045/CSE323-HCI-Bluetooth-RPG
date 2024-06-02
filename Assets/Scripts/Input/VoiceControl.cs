using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class VoiceControl : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();

    private Rigidbody2D rb;
    private Animator anim;
    public float MovementSpeed;
    private Vector2 MovementInput;

    public bool canOpen;

    //private float Horizontal;
    //private float Vertical;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canOpen = false;
        //Horizontal = 0;
        //Vertical = 0;
    }

    void Start()
    {
        actions.Add("move forward", MoveForward);
        actions.Add("move backward", MoveBackward);
        actions.Add("move left", MoveLeft);
        actions.Add("move right", MoveRight);
        actions.Add("stop", StopMoving);
        actions.Add("open chest", OpenChest);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        actions[args.text].Invoke();
    }

    private void MoveForward()
    {
        MovementInput = new Vector2(0, 1);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void MoveBackward()
    {
        MovementInput = new Vector2(0, -1);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void MoveLeft()
    {
        MovementInput = new Vector2(-1, 0);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void MoveRight()
    {
        MovementInput = new Vector2(1, 0);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }

    private void StopMoving()
    {
        MovementInput = new Vector2(0, 0);
        Animate();
        rb.velocity = MovementInput * MovementSpeed * Time.fixedDeltaTime;
    }
    private void OpenChest()
    {
        canOpen = true;
        StartCoroutine(SetCanOpenFalseAfterDelay(5f));
        
    }
    private IEnumerator SetCanOpenFalseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canOpen = false;
    }

    private void Animate()
    {
        anim.SetFloat("MovementX", MovementInput.x);
        anim.SetFloat("MovementY", MovementInput.y);
    }

    void OnApplicationQuit()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}
