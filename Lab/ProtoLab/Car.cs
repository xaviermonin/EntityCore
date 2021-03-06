﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoLab
{
    class Car : ICar
    {
        public Car()
        {
        }

        private ICollection<Wheel> _wheels;

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Wheel> Wheels {
            get
            {
                if (_wheels == null)
                    _wheels = new HashSet<Wheel>();

                return _wheels;
            }
            private set
            {
                _wheels = value;
            }
        }

        private BindingCollection<Wheel, IWheel> bindingCollectionRoues;

        ICollection<IWheel> ICar.Test()
        {
                if (bindingCollectionRoues == null)
                    bindingCollectionRoues = new BindingCollection<Wheel, IWheel>(this.Wheels);

                return bindingCollectionRoues;
        }

        [NotMapped]
        ICollection<IWheel> ICar.Wheels
        {
            get {
                if (bindingCollectionRoues == null)
                    bindingCollectionRoues = new BindingCollection<Wheel, IWheel>(this.Wheels);

                return bindingCollectionRoues;
            }
        }
    }
}
