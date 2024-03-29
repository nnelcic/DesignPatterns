﻿namespace DesignPatterns.Mediator
{
    public interface IMediator
    {
        void Notify(object sender, string ev);
    }

    class ConcreteMediator : IMediator
    {
        private Component1 _component1;

        private Component2 _component2;

        public ConcreteMediator(Component1 component1, Component2 component2)
        {
            _component1 = component1;
            _component1.SetMediator(this);
            _component2 = component2;
            _component2.SetMediator(this);
        }

        public void Notify(object sender, string ev)
        {
            if (ev == "A")
            {
                Console.WriteLine("Mediator reacts on A and triggers folowing operations:");
                _component2.DoC();
            }
            if (ev == "D")
            {
                Console.WriteLine("Mediator reacts on D and triggers following operations:");
                _component1.DoB();
                _component2.DoC();
            }
        }
    }

    class BaseComponent
    {
        protected IMediator _mediator;

        public BaseComponent(IMediator mediator = null)
        {
            _mediator = mediator;
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }
    }

    class Component1 : BaseComponent
    {
        public void DoA()
        {
            Console.WriteLine("Component 1 does A.");

            _mediator.Notify(this, "A");
        }

        public void DoB()
        {
            Console.WriteLine("Component 1 does B.");

            _mediator.Notify(this, "B");
        }
    }

    class Component2 : BaseComponent
    {
        public void DoC()
        {
            Console.WriteLine("Component 2 does C.");

            _mediator.Notify(this, "C");
        }

        public void DoD()
        {
            Console.WriteLine("Component 2 does D.");

            _mediator.Notify(this, "D");
        }
    }

    public class Client
    {
        public void Main()
        {
            Component1 component1 = new Component1();
            Component2 component2 = new Component2();
            new ConcreteMediator(component1, component2);

            Console.WriteLine("Client triggets operation A.");
            component1.DoA();

            Console.WriteLine();

            Console.WriteLine("Client triggers operation D.");
            component2.DoD();
        }
    }
}


namespace DesignPatterns.MediatorV2
{
    public class BodyPart
    {
        private readonly Brain _brain;

        public BodyPart(Brain brain)
        {
            _brain = brain;
        }

        public void Changed()
        {
            _brain.SomethingHappenedToBodyPart(this);
        }
    }

    public class Ear : BodyPart
    {
        private string _sounds = string.Empty;
        public Ear(Brain brain) : base(brain) { }

        public void HearSomething()
        {
            Console.WriteLine("Enter what you hear:");
            _sounds = Console.ReadLine();

            Changed();
        }
        public string GetSounds() => _sounds;
    }

    public class Face : BodyPart
    {
        public Face(Brain brain) : base(brain) { }
        public void Smile() => Console.WriteLine("FACE: Smiling...");
    }

    public class Brain
    {
        public Ear Ear { get; private set; }
        public Face Face { get; private set; }

        public Brain()
        {
            CreateBodyParts();
        }

        private void CreateBodyParts()
        {
            Ear = new Ear(this);
            Face = new Face(this);
        }

        public void SomethingHappenedToBodyPart(BodyPart bodyPart)
        {
            if (bodyPart is Ear)
            {
                string heardSounds = ((Ear)bodyPart).GetSounds();

                if (heardSounds.Contains("cool"))
                {
                    Face.Smile();
                }
            }
        }
    }

    public class Client
    {
        public void Main()
        {
            Brain brain = new Brain();
            Ear ear = new Ear(brain);
            ear.HearSomething();
        }
    }
}