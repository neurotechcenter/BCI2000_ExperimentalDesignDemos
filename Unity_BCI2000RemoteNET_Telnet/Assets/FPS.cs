<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class FPS : MonoBehaviour
{
  [SerializeField] private Text _fpsText;
  [SerializeField] private float _hudRefreshRate = 1f;

  private float _timer;

  private void Update()
  {
    if (Time.unscaledTime > _timer)
    {
      int fps = (int)(1f / Time.unscaledDeltaTime);
      _fpsText.text = "FPS: " + fps;
      _timer = Time.unscaledTime + _hudRefreshRate;
    }
  }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class FPS : MonoBehaviour
{
  [SerializeField] private Text _fpsText;
  [SerializeField] private float _hudRefreshRate = 1f;

  private float _timer;

  private void Update()
  {
    if (Time.unscaledTime > _timer)
    {
      int fps = (int)(1f / Time.unscaledDeltaTime);
      _fpsText.text = "FPS: " + fps;
      _timer = Time.unscaledTime + _hudRefreshRate;
    }
  }
}
>>>>>>> 6cd8fda5fc89e87428191f4287ad6fae25c863ea
