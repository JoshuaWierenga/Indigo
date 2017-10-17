using Indigo.Core.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indigo.Client.ViewModels
{
    public class PageViewModel : ObservableObject
    {
        string _PageName;
        public string PageName
        {
            get => _PageName;
            set => SetProperty(ref _PageName, value);
        }

        string _PageMessage;
        public string PageMessage
        {
            get => _PageMessage;
            set => SetProperty(ref _PageMessage, value);
        }

        public PageViewModel()
        {
            PageName = "Page Name Here";
            PageMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.Suspendisse rhoncus, nisi ut consequat fringilla, lectus sem porttitor arcu, non pretium odio tellus in lectus.Vivamus rutrum imperdiet tincidunt. Sed dolor elit, laoreet a massa at, consectetur aliquet erat. Mauris pulvinar quam in ipsum tempus, bibendum convallis turpis vulputate.Nulla id lorem bibendum, sollicitudin nulla quis, varius felis.Nullam rutrum ullamcorper enim eu bibendum. Fusce sit amet arcu feugiat, laoreet odio sed, pretium urna." +
                "Donec sed quam vitae ex iaculis sollicitudin a vulputate justo. Aliquam risus ante, iaculis lobortis lectus vel, varius aliquam mi. Suspendisse potenti. Phasellus imperdiet lorem justo, quis rhoncus nulla rutrum eu. Suspendisse fermentum pharetra est. In nec nisi leo. Sed eu est auctor, vestibulum ante in, tincidunt ligula. Sed rutrum, metus vitae tempus cursus, velit ante sollicitudin erat, eu elementum urna neque ac lacus.Nulla facilisi. Nullam ut egestas magna." +
                "Donec eget urna in lectus imperdiet varius.Praesent accumsan urna nec est congue gravida.Fusce malesuada commodo neque at posuere. Nunc id quam iaculis, consectetur lorem at, porttitor nisi.Vivamus et est purus. Cras magna nulla, viverra eget nisl sed, vestibulum condimentum nunc. Nam libero augue, vestibulum vel ultrices a, feugiat ac metus." +
                "Nullam ullamcorper nunc at mi interdum, nec molestie urna porta.Integer at sapien mi. Nam commodo ex ligula, ac finibus arcu suscipit ut. Proin id euismod lorem. Duis id massa eros. Nullam egestas orci a nulla elementum, in pretium risus aliquet.Morbi vel arcu ut nulla fringilla tincidunt.Aenean nec nibh blandit, aliquam diam id, pulvinar quam.Sed scelerisque ex a nibh ultricies ultrices.In hac habitasse platea dictumst.Quisque posuere faucibus odio, in vestibulum lorem sollicitudin non. Suspendisse eu massa mauris. Integer tincidunt, ante a efficitur tempus, enim massa aliquam libero, a suscipit turpis lorem et risus." +
                "Vivamus ornare, metus quis aliquet condimentum, tortor est efficitur quam, quis ultrices nibh sem eu felis.Vivamus condimentum hendrerit vehicula. Nam ultricies lobortis elementum. Donec varius imperdiet elementum. Suspendisse eget viverra risus. Vestibulum in nisl at ex blandit pellentesque.Pellentesque quis ornare ipsum. Sed facilisis condimentum ante, eu ultricies magna dignissim vel. Quisque in diam est. Morbi vestibulum euismod neque eget vulputate. Etiam a libero eu ligula convallis accumsan.Proin vitae congue risus. Suspendisse eu mi fringilla, sodales ex sit amet, sodales mauris.";

        }
    }
}