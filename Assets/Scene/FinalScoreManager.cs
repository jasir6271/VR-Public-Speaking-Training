using UnityEngine;
using TMPro;

public class FinalScoreManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI performanceText;

    void Start()
    {
        float score = CalculateScore();

        finalScoreText.text = "FINAL SCORE = " + score.ToString("F0") + "%";
        performanceText.text = GetPerformanceLabel(score);
    }
    float CalculateScore()
    {
        float wpmScore = ScoreWPM(SessionData.WPM);

        // 🔥 FIXED LOUDNESS (more realistic scaling)
        float loudScore = Mathf.Clamp(SessionData.Loudness * 100f, 0f, 20f);

        // 🔥 FIXED PITCH (smooth scoring instead of binary)
        float pitchScore = ScorePitch(SessionData.Pitch);

        // 🔥 FIXED EYE CONTACT (full 0 to 25 scaling)
        float eyeScore = Mathf.Clamp(SessionData.EyeContact * 0.25f, 0f, 25f);

        // 🔥 REDUCED PENALTY
        float fillerPenalty = SessionData.FillerCount * 1f;

        float total = wpmScore + loudScore + pitchScore + eyeScore - fillerPenalty;

        return Mathf.Clamp(total, 0, 100);
    }

    // 🔥 FIXED WPM RANGE (for real speaking)
    float ScoreWPM(float wpm)
    {
        if (wpm >= 80 && wpm <= 140) return 25;   // ideal
        if (wpm >= 60 && wpm < 80) return 20;     // slightly slow
        if (wpm > 140 && wpm <= 180) return 20;   // slightly fast
        return 10;                                // poor
    }

    // 🔥 NEW PITCH SCORING
    float ScorePitch(float pitch)
    {
        if (pitch >= 120 && pitch <= 220) return 20;  // ideal
        if (pitch >= 90 && pitch < 120) return 15;
        if (pitch > 220 && pitch <= 260) return 15;
        return 10;
    }

    string GetPerformanceLabel(float score)
    {
        if (score < 30) return "Needs Improvement";
        if (score < 50) return "Fair";
        if (score < 70) return "Good";
        if (score < 85) return "Very Good";
        return "Excellent";
    }
}