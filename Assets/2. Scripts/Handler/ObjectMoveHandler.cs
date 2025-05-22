using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectMoveHandler : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private List<Transform> waypoints;

    [SerializeField] private float moveDuration = 5f;
    [SerializeField] private Ease ease = Ease.InOutSine;
    [SerializeField] private bool loop = false;

    private Sequence moveSequence;
    private bool isMoving = false;

    public void StartMoving()
    {
        if (isMoving || waypoints == null || waypoints.Count < 2)
            return;

        isMoving = true;
        moveSequence = DOTween.Sequence();

        float segmentDuration = moveDuration / (waypoints.Count - 1);

        foreach (var point in waypoints)
        {
            if (transform.position == point.position)
                continue;

            moveSequence.Append(transform.DOMove(point.position, segmentDuration).SetUpdate(UpdateType.Fixed).SetEase(ease));
        }

        if (loop)
            moveSequence.SetLoops(-1, LoopType.Yoyo);

        moveSequence.OnComplete(() => isMoving = false);
    }
}