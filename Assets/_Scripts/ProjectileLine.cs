using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{
    // static list of active projectile lines; made to follow naming conventions
    static List<ProjectileLine> _projLines = new List<ProjectileLine>();
    private const float DimMult = 0.75f;

    private LineRenderer _line;
    private bool _drawing;

    [Tooltip("RigidBody2D of the projectile. If empty the script will try to find a Rigidbody2D on a parent at runtime.")]
    public Rigidbody2D projectileRb;

    [Tooltip("If true, the line will start drawing automatically when the projectile wakes. If false, call StartDrawing() manually.")]
    public bool startOnAwake = true;

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;

        Vector3 startPos = (projectileRb != null) ? projectileRb.transform.position : transform.position;
        _line.SetPosition(0, startPos);

        if (projectileRb == null)
            projectileRb = GetComponentInParent<Rigidbody2D>();

        if (projectileRb != null && startOnAwake && projectileRb.IsAwake())
            _drawing = true;

        ADD_LINE(this);
    }

    void FixedUpdate()
    {
        // if not drawing yet, start when the projectile wakes
        if (!_drawing && projectileRb != null && startOnAwake && projectileRb.IsAwake())
        {
            // reset and begin drawing from current position
            _line.positionCount = 1;
            _line.SetPosition(0, projectileRb.transform.position);
            _drawing = true;
        }

        if (_drawing)
        {
            Vector3 pos = (projectileRb != null) ? projectileRb.transform.position : transform.position;

            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, pos);

            // stop drawing if the rigidbody falls asleep
            if (projectileRb != null)
            {
                if (!projectileRb.IsAwake())
                {
                    _drawing = false;
                    projectileRb = null;
                }
            }
        }
    }

    /// <summary>
    /// Start drawing the projectile line. Optionally provide the Rigidbody2D to track.
    /// Useful if you want to call this from code (for example, from Ball.Launch).
    /// </summary>
    public void StartDrawing(Rigidbody2D rb = null)
    {
        if (rb != null) projectileRb = rb;
        if (projectileRb == null)
            projectileRb = GetComponentInParent<Rigidbody2D>();

        // reset the line
        _line.positionCount = 1;
        Vector3 startPos = (projectileRb != null) ? projectileRb.transform.position : transform.position;
        _line.SetPosition(0, startPos);
        _drawing = true;

        if (!_projLines.Contains(this))
            ADD_LINE(this);
    }

    private void OnDestroy()
    {
        _projLines.Remove(this);
    }

    static void ADD_LINE(ProjectileLine newLine)
    {
        Color col;
        // dim existing lines
        foreach (ProjectileLine pl in _projLines)
        {
            col = pl._line.startColor;
            col = col * DimMult;
            pl._line.startColor = pl._line.endColor = col;
        }

        _projLines.Add(newLine);
    }
}
