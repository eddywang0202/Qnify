var path = require('path');
var CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
  entry: './src/index.tsx',
  output: {
    path: path.resolve(__dirname, 'build'),
    filename: 'app.js',
    publicPath: '/'
  },
  devServer: {
    historyApiFallback: true
  },
  plugins: [
    new CopyWebpackPlugin([
      { from: 'images/**/*', to: 'images/[hash].[ext]' },
      { from: './index.html', to: '[name].[ext]' },
    ])
  ],
  module: {
    rules: [
      {
        test: /\.(js|jsx|tsx|ts)$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: [
              '@babel/preset-env',
              '@babel/preset-react',
              '@babel/preset-typescript'
            ],
            plugins: [
              '@babel/plugin-proposal-class-properties',
              '@babel/plugin-transform-runtime',
              '@babel/plugin-proposal-export-default-from',
              '@babel/plugin-transform-typescript',
              'babel-plugin-root-import'
            ]
          }
        }
      },
      {
        test: /\.(eot|woff|woff2|ttf|svg|gif)$/,
        use: {
          loader: 'url-loader',
          options: {
            limit: 100000,
            name: '[name].[ext]'
          }
        }
      },
      {
        test: /\.s|css$/,
        loaders: ['style-loader', 'css-loader', 'sass-loader']
      },
      {
        test: /\.(png|jpg|jpeg|ico)$/,
        loader: 'file-loader',
        exclude: [
          path.resolve(__dirname, './node_modules'),
        ],
        options: {
          name: '[path][hash].[ext]',
          publicPath: '/'
        }
      }
    ]
  },
  resolve: {
    extensions: ['.js', '.jsx', '.tsx', '.ts'],
    alias: {
      '~': path.join(__dirname, 'src'),
      'components': path.join(__dirname, 'src/components'),
      'interfaces': path.join(__dirname, 'src/common/interfaces'),
      'server': path.join(__dirname, 'src/server'),
      'utils': path.join(__dirname, 'src/utils'),
    }
  },
}