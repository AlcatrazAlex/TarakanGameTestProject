using System.Collections;
using NUnit.Framework;
using TaranaGame.Logic;
using TaranaGame.Logic.States;
using TaranaGame.Tarakan;
using TaranaGame.UI;
using UnityEngine;
using UnityEngine.TestTools;

namespace TarakanGame.Tests
{
    [TestFixture]
    public class TarakanTests
    {
        private GameObject _tarakanObj;
        private TarakanController _controller;
        private Rigidbody2D _rb;
        private GameSettings _settings;

        [SetUp]
        public void Setup()
        {
            var tarakanId = "BaseTarakan";
            _tarakanObj = new GameObject("Tarakan");
            _controller = _tarakanObj.AddComponent<TarakanController>();
            _controller.transform.position = Vector3.zero;
            _settings = new GameSettings();
            _settings.SetOption(OptionType.Speed, 5);
            _settings.SetOption(OptionType.Acceleration, 10);
            _settings.SetOption(OptionType.CursorSize, 1);
            _settings.FinishDistance = 0.1f;
            _settings.EscapeSpeedMultiplier = 2f;
            _settings.EscapeAccelerationMultiplier = 2f;
            _rb = _tarakanObj.GetComponent<Rigidbody2D>();
            _controller.Init(_settings, Camera.main);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_tarakanObj);
        }

        [Test]
        public void Check_GoToEscapingState_WhenMouseNear()
        {
            _settings.SetOption(OptionType.CursorSize, 5f);
            _controller.SetState(new MovingState());
            _controller.transform.position = Vector2.zero;
            _controller.UpdateMousePosition(Vector2.zero);
            
            _controller.Update();
            
            Assert.IsInstanceOf<EscapingState>(_controller.CurrentState);
        }

        [Test]
        public void Check_SetMovingState_WhenMouseEscaped()
        {
            _controller.SetState(new EscapingState() { EscapeTimer = 1f});
            _controller.UpdateMousePosition(Vector2.one * 100f);
            
            _controller.Update();
            
            Assert.IsInstanceOf<MovingState>(_controller.CurrentState);
        }

        [UnityTest]
        public IEnumerator Check_StayInsideScreenBounds()
        {
            var outOfBoundsPos = Camera.main.ViewportToWorldPoint(new Vector2(1.5f, 1.5f));
            _controller.transform.position = outOfBoundsPos;
            
            yield return new WaitForFixedUpdate();
            
            var viewportPos = Camera.main.WorldToViewportPoint(_controller.transform.position);
            Assert.LessOrEqual(viewportPos.x, 1.0f);
            Assert.LessOrEqual(viewportPos.y, 1.0f);
        }

        [UnityTest]
        public IEnumerator Check_Accelerate_WhenEscaping()
        {
            _controller.SetState(new EscapingState());
            _controller.SetTarget(Vector2.left * 10f);
            _controller.FixedUpdate();
            yield return new WaitForFixedUpdate();
            var speedAfterFrame = Mathf.Abs(_rb.linearVelocity.magnitude);
            
            Assert.Greater(speedAfterFrame, 0f);
        }
    }
}