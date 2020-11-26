import React from "react";
import Slider, { Settings } from "react-slick";

interface ICarouselProps {
  onSlideAfterChange: (currentSlide: number) => void
}

export default class Carousel extends React.PureComponent<ICarouselProps> {

  slider: Slider;

  next = (event: React.MouseEvent<HTMLButtonElement>) => {
    this.slider.slickNext();
  }

  prev = (event: React.MouseEvent<HTMLButtonElement>) => {
    this.slider.slickPrev();
  }

  render() {

    const { onSlideAfterChange } = this.props;

    var settings: Settings = {
      infinite: false,
      speed: 500,
      slidesToShow: 1,
      slidesToScroll: 1,
      className: 'carousel',
      dots: false,
      arrows: false,
      draggable: false,
      accessibility: true,
    };

    return (
      <Slider ref={c => (this.slider = c)} {...settings} afterChange={onSlideAfterChange}>
        {this.props.children}
      </Slider>
    );
  }
}
